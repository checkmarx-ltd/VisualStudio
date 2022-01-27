#!/groovy
@Library('cx-jenkins-pipeline-kit')

def ipAddress
def vmName = "Plugin-VisualStudio-" + UUID.randomUUID().toString()
def msbuildLocation = "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Professional\\MSBuild\\Current\\Bin\\MSBuild.exe""

pipeline {
    parameters {
        string(name: "decommissionPeriod", defaultValue: "3 hour", description: "Decommission period")
		string(name: "templateName", defaultValue: "VisualStudio-Template", description: "Template name, if not specified then \"VisualStudio-Template\" template will be chosen")
        booleanParam(name: 'doNotDeleteVM', defaultValue: false, description: 'If selected, VM will not be deleted after process finished')
		gitParameter branchFilter: 'origin/(.*)', defaultValue: 'master', name: 'branch', type: 'PT_BRANCH'
    }
    agent { node { label 'install01' } }
    stages {
        stage('Create VM') {
            steps {
                script {
                    kit.Create_Vm_Terraform(vmName, templateName, "18000", "8", "VMWARE", decommissionPeriod, "Auto", "Platform-Plugins-Developers")
                    ipAddress = kit.getIpAddress(vmName, "VMWARE")
                    kit.Create_Jenkins_Slave_On_Master(vmName)
                    kit.Start_Jenkins_Slave_On_Windows_Pstools(ipAddress, vmName)
                }
            }
        }

        stage('Build and Pack') {
            agent { node { label vmName } }
            steps {
                script {
                    bat "${msbuildLocation} ${WORKSPACE}\\${JOB_NAME}\\ci.msbuild /t:CI /p:VisualStudioVersion=\"15.0\" /p:DeployExtension=false"
                }
            }
        }
		
		stage('Upload To Artifactory') {
            agent { node { label vmName } }
            steps {
                script {
		    fileOperations([folderRenameOperation(source: "${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX.vsix", destination: "${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX-9.00.8.vsix")])
                    kit.Upload_To_Artifactory("${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX-9.00.8.vsix", "plugins-release-local/com/checkmarx/visual-studio/")
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
                        if (doNotDeleteVM == 'true') {
                            kit.Info_Msg("Not deleting VM since user chose to keep it")
						} else {
							deleteVm("VMWARE", ipAddress, vmName)
						}
                    } catch (Exception e) {
                        kit.Warning_Msg("Failed to delete vm. Exception:\n" + e.toString())
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
