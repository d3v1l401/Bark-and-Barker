# Bark & Barker
##### _Dark & Darker Server Emulator_

A recreated server of Dark & Darker; this server emulators aims to support online multiplayer game sessions.

### Supported

- Full backend operations
    - Logins (Steam)
    - Character management
        - Creation
        - Listing
        - Deletion
    - Matchmaking
    - Party
    - Merchants
    - Character classes & skills
    - Trading posts
    - Chats

### NOT Supported (yet)

- Game Raid sessions

## Installation

`Dockerfile` for settings reference.

```sh
docker build . -t bab
docker run -d bab
```