pipeline {
    agent any

    environment {
        // general
        K8S_DEPLOYMENT_NAME                     = "{{camel}}-deployment"
        DOCKER_NAME                             = "{{camel}}"
        APP_NAME                                = "{{camel}}"
        DOCKER_VERSION_TAG                      = "${BUILD_NUMBER}"
        DOTNET_SOLUTION                         = "{{pascal}}Service.sln"
        MAZECARE_REGISTRY_CREDENTIAL_ID         = "mazecare-ci-cd"
        MAZECARE_IMAGE_REPO_PATH                = "asia.gcr.io/mazecare-cicd"

        // dontia-dev
        DONTIA_DEV_GKE_CREDENTIAL_ID        = "dontia-tech-service-account"
        DONTIA_DEV_GC_PROJECT               = "dontia-alliance"
        DONTIA_DEV_GKE_REGION               = "asia-southeast1"
        DONTIA_DEV_GKE_CLUSTERNAME          = "dontia-dev"
        DONTIA_DEV_CONFIG_PATH              = "infra/dontia-dev"

        // myhc-dev
        MYHC_DEV_GKE_CREDENTIAL_ID        = "myhc-ci-cd"
        MYHC_DEV_GC_PROJECT               = "mhc1-348315"
        MYHC_DEV_GKE_REGION               = "asia-southeast1"
        MYHC_DEV_GKE_CLUSTERNAME          = "myhc-dev"
        MYHC_DEV_CONFIG_PATH              = "infra/myhc-dev"
    }
    
    stages {
        stage ('Docker image build') {
            steps {
                script {
                    sh 'docker image prune -a -f --filter "until=24h"'
                    sh 'docker container prune -f --filter "until=24h"'
                    sh 'docker volume prune -f --filter "label!=keep"'
                    sh 'docker network prune -f --filter "until=24h"'
                    
                    sh "docker build --no-cache --build-arg APP_NAME=${APP_NAME} -t ${DOCKER_NAME} ."

                    sh "docker tag ${DOCKER_NAME} ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}:$DOCKER_VERSION_TAG"
                    sh "docker tag ${DOCKER_NAME} ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}"
                }
            }
        }

        stage ('Push to Mazecare Container Registry') {
            steps {
                script {
                    withCredentials([file(credentialsId: env.MAZECARE_REGISTRY_CREDENTIAL_ID, variable: 'JSON_KEY')]) {
                        sh "gcloud auth activate-service-account --key-file $JSON_KEY"
                        sh "gcloud auth configure-docker"
                    
                        sh "docker push ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}:${DOCKER_VERSION_TAG}"
                        sh "docker push ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}"
                        sh "docker rmi -f ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}:${DOCKER_VERSION_TAG}"
                        sh "docker rmi -f ${MAZECARE_IMAGE_REPO_PATH}/${DOCKER_NAME}"
                        sh "docker rmi -f ${DOCKER_NAME}"
                    }
                }
            }
        }

        stage('Deploy to dontia-dev') {
            steps {
                script {
                    withCredentials([file(credentialsId: env.DONTIA_DEV_GKE_CREDENTIAL_ID, variable: 'JSON_KEY')]) {
                        sh "gcloud auth activate-service-account --key-file $JSON_KEY"
                        sh "gcloud container clusters get-credentials ${DONTIA_DEV_GKE_CLUSTERNAME} --zone ${DONTIA_DEV_GKE_REGION} --project ${DONTIA_DEV_GC_PROJECT}"

                        sh "kubectl apply -f ${DONTIA_DEV_CONFIG_PATH}/k8s.yaml"
                        sh "kubectl rollout restart deployment/${K8S_DEPLOYMENT_NAME}"

                        sh "kubectl rollout status deployment/${K8S_DEPLOYMENT_NAME}"
                    }
                }
            }
        }

        stage('Deploy to myhc-dev') {
            steps {
                script {
                    withCredentials([file(credentialsId: env.MYHC_DEV_GKE_CREDENTIAL_ID, variable: 'JSON_KEY')]) {
                        sh "gcloud auth activate-service-account --key-file $JSON_KEY"
                        sh "gcloud container clusters get-credentials ${MYHC_DEV_GKE_CLUSTERNAME} --zone ${MYHC_DEV_GKE_REGION} --project ${MYHC_DEV_GC_PROJECT}"

                        sh "kubectl apply -f ${MYHC_DEV_CONFIG_PATH}/k8s.yaml"
                        sh "kubectl rollout restart deployment/${K8S_DEPLOYMENT_NAME}"

                        sh "kubectl rollout status deployment/${K8S_DEPLOYMENT_NAME}"
                    }
                }
            }
        }

        stage ('delete workspace') {
            steps {
                deleteDir()
            }
        }
    }
}
