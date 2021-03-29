# HoloTest

Greetings! This repository shows how you can set up automated unit testing for Unity and the Mixed Reality Toolkit. There is a demo Unity project in the src/ folder which simply provides something to test. 

## Ingredients

- Unity 2020.3.0f1 (should work with 2019.3+ too - untested)
- MRTK 2.6.1
- (optional) Jenkins 2.283 (v.284 broke some plugins for me)

----

## Perception Simulation Testing Status

[x] Can deploy the demo app to the emulator and use a PerceptionSim script to do something

[x] Get the PerceptionSimulation plugin to work with Unity

[x] Can run PerceptionSim from within Unity, using Holographic Remoting

[x] Integrate PerceptionSim testing into Playmode testing and run them at the same time

[x] Alternatively, see if input simulation in MRTK is a viable option

[x] Another option - 'Simulate in Editor' uses percsim, I believe. 

### Issues found while prototyping percsim

- TBD

----

## Future Work

[x] Create or recycle a PlayMode test utility. You can test MRTK input simulation in PlayMode, but there is a lot of setup and teardown involved. I made the mistake of simply calling PlayModeTestUtilities.Setup while prototyping these tests. I'm pretty sure that utility was made exclusively for MRTK developers - not for MRTK end users. (For example, it doesn't look like their Setup function allows you to load an existing scene. Leads me to believe it is just an internal utility.) A bit of investigation is needed to find the best way to repurpose this code. 

[x] Once the setup/teardown is ready - add examples that utilize that. 

[x] Jenkins should show you the test results. NUnit does not seem to like that I am claiming that Unity test results are the same as NUnit test results. Some formatting is needed, probably. (For now, I view test results through the build logs folder mentioned in the Jenkinsfile.)

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
