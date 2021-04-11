# HoloTest

Greetings! This repository shows how you can set up automated unit testing for Unity and the Mixed Reality Toolkit. There is a demo Unity project in the src/ folder which simply provides something to test. 

## Ingredients

- Unity 2020.3.0f1 (should work with 2019.3+ too - untested)
- MRTK 2.6.1
- (optional) Jenkins 2.283 (v.284 broke some plugins for me)

----

## Perception Simulation Testing Status

**[Y] Can deploy the demo app to the emulator during Playmode tests**

There are a few ways to do this, but I got it to work by switching the platform to Standalone (instead of UWP) and then clicking on 'Run All Tests (Standalone Windows)' in the Test Runner. The other platform says 'Run All Tests (WSAPlayer)' but fails to load that player. 

"WSAPlayer is the name Unity uses for its UWP standalone player." 

In this way, you can run the playmode tests in the emulator. I think it uses a PerceptionSim headset and input, so this can be utilized in these tests. Needs more tinkering, though. 

**[N] Get the PerceptionSimulation plugin to work with Unity**

**[N] Can run PerceptionSim from within Unity, using Holographic Remoting**

(Would this even use PS? Or would it just use Unity input simulation and send that to the device?)

**[N] Integrate PerceptionSim testing into Playmode testing and run them at the same time**

**[Y] Alternatively, see if input simulation in MRTK is a viable option**

An example of this is included in the src/ project, in the PlayMode tests. This is a good way to write HoloLens interface tests (such as hand interactions), but more investigation is needed to see how 'accurate' the tests are compared to those that use PerceptionSimulation. 

**[N] Another option - try 'Simulate in Editor' - does this use the PerceptionSimulation API?**

----

Interesting discussion about end-to-end testing using PercSim in this thread:
https://forums.hololens.com/discussion/3239/hololens-end-to-end-tests

----

## Unity Test Framework

You can explore this framework through the Test Runner window in Unity. There are two main types of tests that you can create - edit mode and play mode tests. 

### Edit Mode Testing Overview

These tests run inside the Unity Editor, or use Editor code. I've included a few examples in this repository, but you can find many awesome tests in the MRTK Test packages. (MixedRealityToolkit.Tests.EditMode)

Examples of things that are testable in Edit mode: 

- Editor scene management
- Checking assets, game objects, and initial properties of things
- Calculations that don't require runtime objects or behavior

Edit mode tests are also ultra-quick to run.

### Play Mode Testing Overview

This is exactly what it sounds like: runtime testing. These tests take longer to run, especially if you are loading complex scenes and creating new game objects. 

Examples of things that are testable in Play mode: 

- MRTK interactions (especially using the kit's UX prefabs)
- Configuration profile switching at runtime (attached to the MixedRealityToolkit game object)
- Interactable event tests - toggle, click, etc. 
- Shader tests
- Animations
- Particle Systems

Anything that is scriptable is fair game. PlayMode tests are extremely powerful, and all of the heavy lifting - such as scene management and MRTK configuration - can be tucked away into a test suite's Setup & TearDown functions. 

----

## Jenkins Setup (optional)

I've included these steps for future reference, but naturally, you do not need the Jenkins server to run unit tests. 

All of the Jenkins steps are grouped into a Pipeline script named Jenkinsfile. (Pipeline is a DSL based on Groovy, I think.) This script interacts with the suite of plugins installed on the server. Pipelines can be as simple or powerful as the user wants, and can pretty much do anything, which is awesome. 

There is also a build script called JenkinsBuild.cs. 

Jenkins and Unity use this file to build the project - see the 'Build' stage in the Jenkinsfile for more information. 

Note: it looks like you need to pass in the test platform through Jenkins, and not through the CSharp build script. This is not an option for batchmode Unity as far as I can tell.

### Assumptions
- The Jenkinsfile assumes the source code is hosted on GitHub. This can be configured in the Checkout stage. 

### Preflight Checklist
Inside Jenkins, install the required plugins: 
1. Credentials Plugin
2. Git Plugin
3. GitHub Plugin
4. All of the Pipeline-related plugins. (including Declarative)
5. Generic Webhook Trigger Plugin
6. Build With Parameters
7. xUnit
8. Workspace Cleanup

I've also included a full list of plugins that are installed on my server, in this repo's automation directory. 
There are a lot of Jenkins plugins, and many do similar things, so in retrospect it is hard to identify which ones are actually being utilized.

Other setup steps:

- [Jenkinsfile] Check the Unity installations against those listed in the Environment section.
- [CS Build Script] Change any parameters in here as needed. (Called JenkinsBuild.cs)
- [Jenkinsfile] Change the Git URL in the Checkout stage to match the new source repository. 
- [Jenkins Settings] Add user credentials, either as a direct account, or with SSH and a private key. 
- [Jenkinsfile] Change 'credentialsId' field to match the one you set in Jenkins. 
- [Jenkins Settings] Set up a custom workspace - some target directory on the build machine.

There are some other Jenkins steps not included here. You would need to set up a Pipeline project, then tell Jenkins where to find the Jenkinsfile, etc. Since these are easy to find on the internet, I won't re-type them here, but let me know if you run into any trouble.

----

## Future Work 

[x] Jenkins should show you the test results. NUnit does not seem to like that I am claiming that Unity test results are the same as NUnit test results. Some formatting is needed, probably. (For now, I view test results through the build logs folder mentioned in the Jenkinsfile.)