# 🧪 Laboratory Work 1: Variant 8
Зробив студент групи Б-121-24-3 ПІ
Рудий Іван Володимирович
## Designing a Messaging System
### 🎯 Goal
Learn how to:
- design software systems before coding;
- reason about architecture and responsibilities;
- use Component, Sequence, and State diagrams;
- document decisions using RFC and ADR.

---

## 🧠 Context

You are designing a minimal messenger system that supports:
- sending messages between users;
- asynchronous delivery;
- message statuses (sent / delivered / read);
- offline users.

---

## 🧩 Functional Requirements
1. A user can send a message to another user.
2. Each message has a lifecycle.
3. The system must:
  - store messages,
  - deliver them asynchronously,
  - update delivery status.
4. The recipient may be online or offline.

---

## 🧱 Part 1 — Component Diagram
In an E2EE system, we must introduce a Key Directory component. The server stores public keys, but the private keys never leave the clients.

### Task
```mermaid
graph LR

    ClientA -->|1. Request B's Public Key| API
    API <--> KeyDirectory
    ClientA -->|2. Send Encrypted Payload| API
    
    API --> MsgService
    MsgService -->|3. Store Ciphertext| DB
    MsgService -->|4. Enqueue for B| Queue
    Queue --> Delivery
    
    Delivery -.->|5. Push Ciphertext| ClientB
    ClientB -.->|6. Decrypt Locally| ClientB
```

---

## Part 2 — Sequence Diagram
This sequence illustrates the specific scenario requested: User A sends a message to User B, who is currently offline. Notice where the encryption happens to ensure the server never sees plain text.

### Task
```mermaid
sequenceDiagram
    participant A as User A (Client)
    participant Server as Backend API & Services
    participant DB as Database
    participant B as User B (Client)

    A->>Server: GET /users/B/public-key
    Server-->>A: Returns User B's Public Key
    
    Note over A: A encrypts the message locally<br/>using B's Public Key
    
    A->>Server: POST /messages (Payload: Ciphertext)
    Server->>DB: save(ciphertext, recipient: B, status: Sent)
    Server-->>A: 202 Accepted
    
    Note over Server,B: User B is currently Offline.<br/>Message sits in the queue/DB.
    
    B->>Server: Connects / Comes Online
    Server->>B: Pushes pending Ciphertext
    
    Note over B: B decrypts the message locally<br/>using their Private Key
    
    B->>Server: PUT /messages/{id}/status (Delivered)
    Server->>DB: updateStatus(Delivered)


```

---


## Part 3 — State Diagram
The lifecycle of an E2EE message contains unique states compared to a standard message, specifically regarding the encryption and decryption phases on the client edges.

### Task
```mermaid
stateDiagram-v2
    [*] --> Draft : User typing
    Draft --> Encrypted : Client encrypts content
    Encrypted --> Sent : Dispatched to network
    
    Sent --> Stored : Server DB (Waiting for B)
    Sent --> Failed : Network error
    Failed --> Encrypted : Retry sending
    
    Stored --> Delivered : Pushed to User B's device
    Delivered --> Decrypted : B's device decrypts payload
    Decrypted --> Read : User B views message
    Read --> [*]
```

---

## Part 4 — Architecture Decision Record (ADR)
```markdown
# ADR-002: Implementation of End-to-End Encryption (E2EE) for Direct Messaging

## Status
Accepted

## Context
We are designing a messaging system where user privacy and data security are the highest priorities.
The system must guarantee that the server, database administrators, or any intercepting third parties cannot read the content of the messages.
Furthermore, users can be offline at the time a message is sent to them, requiring temporary server-side storage of the message.

## Decision
  - We will implement asymmetric End-to-End Encryption (E2EE).

  - Clients will generate public/private key pairs locally.

  - Private keys will never leave the client device.

  - The server will host a Key Directory to distribute public keys.

  - Senders will fetch the recipient's public key, encrypt the message payload locally, and transmit only the ciphertext.

## Consequences
+ Guarantees absolute privacy of message content.
+ Protects against data breaches; compromised databases will only yield useless ciphertext.
- Increased complexity in client-side logic (key management, local storage, decryption).
- Potential for complete message loss if a user loses their device and hasn't backed up their private key.
```
