Noise Generators - White & Pink Noise native audio plugins

Thank you for downloading the Noise Generators asset! This asset supports Mac, Windows, iOS, and Android. To use the plugins in your project, follow the platform-specific configuration steps:

MAC
Add ‘AudioPluginNoiseGenerators.bundle’ to the Assets > Plugins folder in your Unity project.

WINDOWS
Windows users will have to first install the Visual C++ Redistributable package for Visual Studio 2015 RC, located here: https://www.microsoft.com/en-us/download/details.aspx?id=46881
Once the Redistributable package has been installed, add ‘AudioPluginNoiseGenerators_x86.dll’ (32 bit) to Assets > Plugins > x86, or ‘AudioPluginNoiseGenerators_x64.dll’ (64 bit) to Assets > Plugins > x86_64.

iOS
Add ‘AudioPluginInterface.h’, ‘AudioPluginNoiseGenerators.h’, and ‘libAudioPluginNoiseGenerators.a’ to Assets > Plugins > iOS. Once you’ve built the Xcode project from Unity, open UnityAppController.h and add the following line before the @interface declaration to import AudioPluginInterface.h:

#import "AudioPluginInterface.h"

Next, open UnityAppController.mm, and in -(void)preStartUnity{}, add the following line:

UnityRegisterAudioPlugin(&UnityGetAudioEffectDefinitions);

ANDROID
Add ‘libAudioPluginNoiseGenerators.so’ to Assets > Plugins > Android.

—————

Using Noise Generators:

1) Create an AudioMixer if you don’t already have one in your project. In the Inspector panel, deselect ‘Enable Suspend’.
2) Select an AudioMixer group (ex: ‘Master’ group), and click ‘Add’. Choose White or Pink Noise.
3) You should be able to hear the plugin synthesizing the corresponding coloured noise. 

Noise Generators parameters 

Gain: Controls the overall volume gain (0.0 - 1.0).
LFO Frequency: Controls the oscillator frequency (0.01 - 20.0 Hz).
LFO Depth: Controls the intensity of the modulation (0.0 - 100.0).

—————

If you have questions or feedback about Noise Generators, please contact the developer at alexander@hodge.io.

