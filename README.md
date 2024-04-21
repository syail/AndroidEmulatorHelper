# AndroidEmulator Helper

Provides Android emulators such as LDPlayer and Bluestacks on Windows,  
functions such as screen capture, automatic list-up, and click are.

## Basic example

```cs
using AndroidEmulatorHelper;

AndroidEmulatorBase[] emulators = BlueStacks.GetList();

for(int i = 0; i < emulators.Length; i++)
{
    Console.WriteLine($"{i}. {emulators[i].GetProcessName()}");
}
Console.Write("Select an emulator: ");

int selectedEmulatorIndex = int.Parse(Console.ReadLine()!);

AndroidEmulatorBase currentEmulator = emulators[selectedEmulatorIndex];

Console.WriteLine(currentEmulator.ToString());

currentEmulator.SendString("Hello, World!").Wait();
currentEmulator.CaptureScreen().Save("screenshot.png");
```

## Future updates

- [ ] x86_64 memory scanning
- [ ] Send ADB command
