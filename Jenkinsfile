pipeline {
    agent any
	
	environment {
		gitlab_user_pwd=credentials('gitlab-user-pwd')
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
	}
}
