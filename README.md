# Bark & Barker
##### _Dark & Darker Backend Server Emulator_

A recreated server of Dark & Darker; this server emulator aimed to emulate the backend server.

Some time ago Nexon sued Ironmace development studio of Dark & Darker, we were worried we'd lose the game once and for all, hence, we started working on a game server emulator.

We are glad Dark & Darker have been released shortly after, nullifying our effort (but we're still glad!).

**WARNING:** This project is incomplete, but it's a great resource to learn how D&D client communicates with its backend.

### Authors

- [Primitheus](https://github.com/Primitheus)
- [d3vil401](https://github.com/d3v1l401)
- [Demonofpower](https://github.com/Demonofpower)

### License

GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007

### Supported

- Full backend operations
    - Logins
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

### NOT Supported

- Game Raid sessions
    - For context: the game server runs over the Unreal Engine proprietary networking, thus, requiring extensive work to rewrite the Unreal Engine network stack which was not within our scope.

## Installation

`Dockerfile` for settings reference.

```sh
docker build . -t bab
docker run -d bab
```
