#!/groovy
@Library('cx-jenkins-pipeline-kit') _

def ipAddress
def templateName = "AppGatedSmokeClean"
def vmName = "Plugin-VisualStudio-" + UUID.randomUUID().toString()
def msbuildLocation = "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\15.0\\Bin\\msbuild.exe\""

pipeline {
    parameters {
        string(name: "ram", defaultValue: "18000", description: "Server memory")
        string(name: "cpu", defaultValue: "8", description: "")
        string(name: "provider", defaultValue: "VMWARE", description: "IAAS platform to be used")
        string(name: "decommissionPeriod", defaultValue: "1 hour", description: "Decommission period")
        booleanParam(name: 'deleteVMinPost', defaultValue: true, description: 'If selected VM will be not deleted after process finished')
		//string(name: "branchName", defaultValue: "master", description: "github branch for Visual Studio plugin")
    }
    agent { node { label 'install01' } }
    options {
        timestamps()
        timeout(time: 3, unit: 'HOURS')
    }
    stages {
        stage('Create VM') {
            steps {
                script {
                    kit.Create_Vm_Terraform(vmName, templateName, ram, cpu, provider, decommissionPeriod, "Auto", "Plugins-Developers")
                    ipAddress = kit.getIpAddress(vmName, provider)
                    kit.Create_Jenkins_Slave_On_Master(vmName)
                    kit.Start_Jenkins_Slave_On_Windows_Pstools(ipAddress, vmName)
                }
            }
        }

        /**
        stage('Download Code from Github') {
            agent { node { label vmName } }
            steps {
                git changelog: false, credentialsId: 'f62744b7-1d3d-41a7-a3a6-cbe679c24b26', poll: false, url: 'https://github.com/CxRepositories/VisualStudio'
            }
        }
        */

        stage('Build plugin') {
            agent { node { label vmName } }
            steps {
                script {
                    bat "${msbuildLocation} ${WORKSPACE}\\Plugin-VisualStudio\\ci.msbuild /t:build /p:VisualStudioVersion=\"15.0\""
                }
            }
        }
		
		stage('Pack plugin') {
            agent { node { label vmName } }
            steps {
                script {
                    bat "${msbuildLocation} ${WORKSPACE}\\${JOB_NAME}\\ci.msbuild /t:pack /p:VisualStudioVersion=\"15.0\""
                }
            }
        }
		
		stage('Upload_To_Artifactory') {
            agent { node { label vmName } }
            steps {
                script {
                    kit.Upload_To_Artifactory("${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX.vsix", "plugins-release-local/com/checkmarx/visual-studio/")
                }
            }
        }
    }

    post {
        always {
            script {
                logstashSend failBuild: false, maxLines: 1000
                if (ipAddress != null) {
                    try {
                        if (deleteVMinPost == 'true') {
                            deleteVm(provider, ipAddress, vmName)
                        }
                    } catch (Exception e) {
                        kit.Warning_Msg("Clean VM - Fail to delete. Exception:\n" + e.toString())
                        currentBuild.result = 'UNSTABLE'
                    }
                }
            }
        }
        cleanup {
            cleanWs()
        }
    }
}
