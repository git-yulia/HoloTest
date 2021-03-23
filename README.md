# HoloTest

## Assumptions
- The Jenkinsfile assumes the source code is hosted on GitHub. This can be configured in the Checkout stage. 

## Preflight Checklist
- [Jenkins] Install required plugins: 
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