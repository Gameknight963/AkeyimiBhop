# AKEYIMI BHOP MOD (First Ever Akeyimi Mod)

## Installation
1. Get the Melonloader Installer from [Here](https://melonwiki.xyz).
2. Run it, Select Add Game Manually, and Point it at your Akeyimi executable.
3. Click Akeyimi And Press Install.
4. Drag this DLL into your MODS folder.

## Configuration
Go to Mods/AkeyimiBhop/MovementConfig.json and edit the Settings there. It's generated at runtime, you will need to run the game at least once for it to appear.

## Building

After cloning, update Directory.build.props to match the path of your Akeyimi instance. Then build it with MSBuild. Files are automatically copied.

### How it works

In essence, it patches `PlayerController.Update`.

From my basic reverse-engineering, `PlayerController.Update()` calls two methods:

1. `this.Move()`
2. `this.Look()`

Our prefix calls `__instance.Look()` to preserve camera movement, but intentionally doesn't call `__instance.Move()` so we can use our own movement. Then it returns false so the original method doesn't run.

The original `Move()` function uses a `PlayerController` to orchestrate movements. We use the same controller, keeping track of our velocity in a field since it doesn't support velocity.

Then it's as simple as calling my reimplementations of Source movement functions.

## Wtf is an akeyimi

https://discord.com/channels/1466959156492767417/1480960594420175010/1541871755713585252