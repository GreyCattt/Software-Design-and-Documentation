🧪 Laboratory Work 1
Designing a Messaging System
🎯 Goal
Learn how to:

design software systems before coding;
reason about architecture and responsibilities;
use Component, Sequence, and State diagrams;
document decisions using RFC and ADR.
🧠 Context
You are designing a minimal messenger system that supports:

sending messages between users;
asynchronous delivery;
message statuses (sent / delivered / read);
offline users.
🧩 Functional Requirements
A user can send a message to another user.
Each message has a lifecycle.
The system must:
store messages,
deliver them asynchronously,
update delivery status.
The recipient may be online or offline.
🧱 Part 1 — Component Diagram
In an E2EE system, we must introduce a Key Directory component. The server stores public keys, but the private keys never leave the clients.
https://mermaid.ai/d/55ad4927-66a6-4fdf-ba94-f45362584bbd
Part 2 — Sequence Diagram
This sequence illustrates the specific scenario requested: User A sends a message to User B, who is currently offline. Notice where the encryption happens to ensure the server never sees plain text.
https://mermaid.live/edit#pako:eNqlVU1v2kAU_CurPQXJ4cNxCFgVEm6qqoe2kZJeKi4O3hDUgKk_orYIqZBE6SFq1Qrl0EvUW49WlDQECvkLu_-os96ASDG9FAlkL-_NzJs3stu06jqMmtRnb0PWrLLNul3z7EalSfBp2V5Qr9ZbdjMgZWL7hH_nE_GRD0RX9MQRv-SROMU_KzgfiXPR52PRSy32bjPvkHkxwFd-zYf4jvkVKW89I6LHIyK6_Bqwl4Do8sFi_6Z13xvxG5TzK1yMoeJksdRaKtNakKm6y6ulkhJokqdPdkjoM89HdSvcPahXV9-w96pO1ayiumwSfsEnwIVqOYDoEykeuoYxbY8P-C3BnCPxGcxJcjCGpXDV7ws3YMSVLgG9TPgI-EOMORJnGHVCxCf0H4PuSJLdSfaYcMJ_o1b6ORZfHu16mRLwB2idMcalR1IIH_xT5gM9865svdzeIZkG8327xnzY2Jf-i3PY8FOpAltPLlbypR7YVSptWibx7UO2Mh1hvlgjkHcVb__X1BqTWBqJlUfSL9EFz7dY9h3KIhSpgSephMXoWR3LkePxW2mJnJ-slKtV1gqYk0q2XCFoELokOWCLYu4bcQ7F4hgSIkmQVpZfJO-DIK6y8yQeEoaLMwDj-JJgG3Jo4GWQ6356Xpg15z1miRemdiSjNkPJEP5jmr4JeO4l_WW_tegeXJEt4hTHw2k0kISkBSUbBlArIaPoxEQzlDh3_5NWPA1k3630MUoM7TLbtl7NJTbTrjudjB_YQajSO1HpWpYkldmw5dgB2467kpuoRmte3aFm4IVMow3mNWx5S9sSrkKDfdZgFWri0mF7dngQVGil2UEbHlWvXbcx7fTcsLZPzT37wMedIr5_EM9KWNNh3mM3bAbUzOn5GIOabfoOt-tGuqCvZ4sFPW-sFfWcrtH3OC6kc8X17IaRz60VjbyxYXQ0-iGmzaYLhayh62v4GkY-m8tplDn1wPWeq9dB_Fbo_AHImRY2
Part 3 — State Diagram
The lifecycle of an E2EE message contains unique states compared to a standard message, specifically regarding the encryption and decryption phases on the client edges.
https://mermaid.live/edit#pako:eNp1k81u00AQx19ltSdATmRv_JH4wCEUblzgBuFg1ZvUUmxHrl1RokhJg1RuFSjKAQl4hcgkUNImeYXZN2J2Nx9tET6svKv5_-Y_M7t9epyGnPr0NA9yfhQFnSyIK2eslRD83j55RyqVp-QoC9o58Ql8hbUYwrUYiQsxhhJm4pLAXAxxsxRjMdEyHS6Fz5Pj7LyX81CLb8RUTGAlLoj4hJSPUigmBJawlqewkKtmHJSS85onKv8XMYU5bNDDDLPfSAGsCZQEbmGBpwv4JcYaoFclVIQ8zbSNzzAhYqTCS7XOyCP4Li4RrWogzccPtC-CqKu1P9DoLVxj5iXK7iSdask28p_Spa7EAmX3VlJZ3isEYfdMa6-q87wbnfGt828YPMcWjjD_TFeuYJv9TIaI_UOa2zHstZp0189DBcGDNfzejUXZVaOZy0y78g4MSXzFg_B_l6KJrrbN-Yljv5IcidsosqxdNlIPcCWuNF4BJRnvHTVoJ4tC6udZwQ0a8ywO5Jb2ZWyL5ic85i3q42_I20HRzVu0lQxQ1guSN2ka75RZWnROqN8Ouqe4K3rh4Z7vQ3gS8uxZWiQ59S2LNRSE-n36nvrMblRN23Utt8aY7Xo1z6Dn1K8wp8oc2_Qcp2Y6tsXq7sCgH1Riq2o13LrFTOYyz2p4Zt2gPIxwqi_1Y1NvbvAXoHmV6A
Part 4 — Architecture Decision Record (ADR)
ADR-002: Implementation of End-to-End Encryption (E2EE) for Direct Messaging

Status
Accepted

Context
We are designing a messaging system where user privacy and data security are the highest priorities. The system must guarantee that the server, database administrators, or any intercepting third parties cannot read the content of the messages. Furthermore, users can be offline at the time a message is sent to them, requiring temporary server-side storage of the message.

Decision
We will implement asymmetric End-to-End Encryption (E2EE).

Clients will generate public/private key pairs locally.

Private keys will never leave the client device.

The server will host a Key Directory to distribute public keys.

Senders will fetch the recipient's public key, encrypt the message payload locally, and transmit only the ciphertext.
Consequences

(+) Guarantees absolute privacy of message content.

(+) Protects against data breaches; compromised databases will only yield useless ciphertext.

(-) Increased complexity in client-side logic (key management, local storage, decryption).

(-) Potential for complete message loss if a user loses their device and hasn't backed up their private key.
