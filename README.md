# HoloTest

Greetings! This repository shows how you can set up automated unit testing for a Unity project that uses Mixed Reality Toolkit. There is a demo Unity project in src/ which simply provides something to test. 

## Ingredients

- Unity 2020.3.0f1 (should also work with 2019.3+, untested for this project)
- MRTK 2.6.1
- (optional) Jenkins 2.283 (.284 broke some plugins for me)

## Unity Test Framework

You can explore this framework through the Test Runner window in Unity. There are two main types of tests that you can create:

1. Edit mode tests - can be used to run tests inside the Unity Editor or using Editor code. I've included a few examples in this repository, but you can find many awesome tests in the MRTK Test packages. 

Examples of things that are testable in Edit mode: 

- Editor scene management
- Checking assets, game objects, and initial properties of things
- Calculations that don't require runtime objects or behavior

And basically anything else that is scriptable. Edit mode tests are also ultra-quick to run.

2. Play mode tests - exactly what it sounds like: tests that require runtime. They take a bit longer to run, especially if you are loading scenes and creating game objects. 

Examples of things that are testable in Play mode: 

- MRTK interactions (especially using the kit's UX prefabs)
- Configuration profile switching at runtime (see MixedRealityToolkit game object for details)
- Interactable event tests - toggle, click, etc. 
- Shader tests
- Animations
- Particle Systems

Again, anything that is scriptable is fair game. PlayMode tests are extremely powerful, and all of the heavy lifting - such as scene management and MRTK configuration - can be tucked away into the test suite's Setup & TearDown functions. 

----

## Jenkins Setup (optional)

I've included these steps for future reference, but naturally, you do not need the Jenkins server to run unit tests. 

All of the Jenkins steps are grouped into a Pipeline script named Jenkinsfile. (Pipeline is a DSL based on Groovy, I think.) This script interacts with the suite of plugins installed on the server. Pipelines can be as simple or powerful as the user wants, and can pretty much do anything, which is awesome. 

There is also a build script called JenkinsBuild.cs. Jenkins and Unity use this file to build the project - see the Build stage in the Jenkinsfile for more information. 

Note: it looks like you need to pass in the test platform through Jenkins, not through the CS build script (this is not an option for batchmode Unity as far as I can tell).

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

I've also included a full list of plugins installed on my server (in this repo's automation directory). 
There are a lot of Jenkins plugins, and many do similar things, so in retrospect it is hard to identify
which ones are actually being utilized. 

- [Jenkinsfile] Check the Unity installations against those listed in the Environment section.
- [Jenkinsfile] Change the Git URL in the Checkout stage to match the new source repository. 
- [Jenkins Settings] Add user credentials, either as a direct account, or with SSH and a private key. 
- [Jenkinsfile] Change credentialsId field to match the one you set in Jenkins. 
- [Jenkins Settings] Set a custom workspace - some directory on the build machine.

----

## Additional Resources

EditMode vs. PlayMode Tests

https://docs.unity3d.com/Packages/com.unity.test-framework@1.0/manual/edit-mode-vs-play-mode-tests.html
