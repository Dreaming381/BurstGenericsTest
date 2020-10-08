# BurstGenericsTest

This is a Unity Project showcasing how generic jobs can and cannot be used with
Burst in the context of a Fluent API.

### How it works

There is a single scene with several cubes. Each cube has a unique MonoBehaviour
attached, all of which inherit from BurstCheckCubeColorizerBase. Each of these
uses a different technique for creating, configuring, and running a generic job
using a processor passed in from the base class. The processor checks if the job
executed in Burst and reports this info to the base class. The base class then
colorizes the cube based on the result.

Green means the job executed in Burst.

Red means the job did not execute in Burst.

Using Unity 2020.1.7f1 and Burst 1.3.8

#### Editor

![](media/7b058c9e9ed0c958d50c6a4c10b97c75.png)

#### Build

![](media/1cb24888c591a148b8cad6c51c6b10b8.png)

The cube on the left uses an *ideal* API design and code structure.

The adjacent cube showcases a common pitfall.

The middle cube shows an ugly way to make the generic job work with Burst. The
API as exposed to the MonoBehaviour was compromised to do this.

The cube to the right of the middle shows how the original API can be recovered
while still working with Burst.

The cube furthest to the right extends these concepts to recover much of the
code organization and structure to the cube on the left. I consider this an
acceptable solution.

### The right cubes work with Type-Inferencing?

Yes!

That seems pretty crazy, especially since if the generic method did the job
scheduling, it wouldn’t have worked with Burst. The key is when you invoke the
generic method, that invocation is concrete. Consequently, the return value is
also concrete. This means that regardless of the implementation inside the
generic method, the compiler knows that you will receive a concrete
Config\<CheckBurstProcessor\> and so it creates a temporary variable for it.
That temporary variable now creates a concrete representation of all subtypes
inside of it, including the generic jobs. That is all Burst needs to detect it,
and the job is able to run in Burst without sacrificing the Fluent API with
type-inferencing. It is actually because this is a Fluent API that the
type-inferencing is even possible.
