pipeline {
    agent any
	
	environment {
		TAG = sh(script: "echo dev-`date +%Y%m%d%H%M`", returnStdout: true).trim()
		gitlab_user_pwd=credentials('gitlab-user-pwd')
		Registry = "registry.gitlab.com/rayprosoft/turnkey-gaming-system/"
        BuildConfiguration="Debug"
	}

    stages {
		stage('Clean') {
            steps {
                cleanWs()
            }
        }
		
		stage('Clone') {
			steps {
				git branch: "master", url: "https://${gitlab_user_pwd_USR}:${gitlab_user_pwd_PSW}@gitlab.com/rayprosoft/notify-api.git"
			}
		}
		
        stage('Build') {
            steps {
                sh 'dotnet build -c $BuildConfiguration'
            }
        }
		
		stage('Push packages') {
			steps {
				sh 'dotnet pack -c $BuildConfiguration "Messages/Messages.csproj" --no-build'
				sh 'dotnet nuget push "Messages/bin/$BuildConfiguration/*.nupkg" --source "raypro" --skip-duplicate'
			}
		}
		
		stage('Deploy') {
		    steps {
				script {
					sh "sudo docker build -f NotifyApi/Dockerfile -t registry.gitlab.com/rayprosoft/turnkey-gaming-system/notify-api:${TAG} ."
					sh "sudo docker build -f NotifyApi/Dockerfile -t registry.gitlab.com/rayprosoft/turnkey-gaming-system/notify-api:dev ."
					sh "sudo docker push registry.gitlab.com/rayprosoft/turnkey-gaming-system/notify-api:${TAG}"
					sh "sudo docker push registry.gitlab.com/rayprosoft/turnkey-gaming-system/notify-api:dev"
							
					withKubeConfig([credentialsId: 'k8s-kube-config', serverUrl: 'https://192.168.50.105:6443']) {
						sh "kubectl set image deployment/notify-api notify-api=${Registry}notify-api:${TAG} --record -n monitoring"
					}
				}
			}
		}
	}
}
