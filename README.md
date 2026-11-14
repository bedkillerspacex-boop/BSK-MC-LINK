# BSK-MC-LINK

Lightweight, dependency-free IPv6 P2P UDP tunnel & offline invite-code link library extracted from PCL-Lite for Minecraft Java LAN multiplayer (**logic & transport only, zero UI dependencies**).

## Features

- **Serverless IPv6 P2P Tunnel (`UdpReliablePeer`)**: Custom reliable, ordered multi-stream transport over UDP with selective ACKs (SACK), RTT tracking, and hole punching (`Ipv6HostSession` / `Ipv6GuestSession`).
- **Offline Invite & ACK Codes (`InviteCodeCodec`)**:
  - `BSK-...` (Host Invite) & `ACK-...` (Guest ACK fallback when host firewall blocks unsolicited UDP).
  - Compact Base58 encoding containing version/kind, IPv6 (16B), UDP port (2B), random 32-bit session token (4B), CRC-16/CCITT (2B), plus a trailing **Luhn mod-58 check character** to catch single-character typos, omissions, or transpositions.
- **Privacy & Port Hygiene**:
  - Host and guest peers are identified only by virtual IDs (`好友 #1`, `房主 #1`) and logs omit/redact raw IPv6 addresses.
  - Guest local proxy (`127.0.0.1:<port>`) and UDP endpoints allocate cryptographically random unoccupied ports (`[20000, 60000)`) with `ExclusiveAddressUse` enabled.
- **LAN Multicast Discovery & Spoofing (`LanBroadcastHelper`)**: Listens to and announces Minecraft Java `[MOTD]...[AD]...[/AD]` multicast frames (`224.0.2.60:4445`).
- **Public IPv6 Detection (`Ipv6AddressDetector`)**: Filters link-local, loopback, ULA, Teredo, and 6to4 addresses, prioritizing active default-route adapters.

## Project Layout

- `src/BskMcLink/` — Pure `.NET 10` class library (`net10.0`, zero NuGet dependencies)
  - `InviteCodeCodec.cs`
  - `Ipv6AddressDetector.cs`
  - `LanBroadcastHelper.cs`
  - `UdpReliableTunnel.cs`
  - `Ipv6DirectService.cs`
  - `DirectLinkLog.cs`
- `tests/BskMcLink.Tests/` — MSTest unit & loopback integration tests

## Build & Test

```bash
dotnet test -c Release
```

## Security Notice

- `BSK-` / `ACK-` codes use Base58 encoding with CRC-16 + trailing check digit for **typo detection**, not encryption. Share invite codes only with trusted friends.
- Host sessions only bridge validated streams to loopback (`127.0.0.1` / `[::1]`) on the configured Minecraft port (not a full virtual LAN).
- Note: Current UDP transport authenticates packets using the 32-bit session token and relies on Minecraft's own protocol encryption when `online-mode=true`, without an additional transport AEAD layer.

## License

[MIT](LICENSE)
