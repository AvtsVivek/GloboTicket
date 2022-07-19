

https://docs.microsoft.com/en-us/cli/azure/
https://docs.microsoft.com/en-us/cli/azure/run-azure-cli-docker
Commands
https://docs.microsoft.com/en-us/cli/azure/group?view=azure-cli-latest
docker 
docker run --name azurecli -it mcr.microsoft.com/azure-cli

Resource Group creation
az group create --location westindia --name Mewurk_Vivek_aks

Azure kubernetes paas service is not available at westindia location
az group create --location centralindia --name Mewurk_Vivek_aks
az group show --name Mewurk_Vivek_aks
az group delete --name Mewurk_Vivek_aks --yes

Azure cli listings. 
az login and az logout
To list locations az account list-locations

Sql
To create a sql server: 

az sql server create --location westindia --resource-group Mewurk_Vivek_aks --name mewurkglobosqlserver -u mewurk_globo_admin -p vivek@123
az sql server show --resource-group Mewurk_Vivek_aks --name mewurkglobosqlserver
To delete: 
az sql server delete --resource-group Mewurk_Vivek_aks --name mewurkglobosqlserver --yes

Create a firewall rule for local ssms access
az sql server firewall-rule create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name ClientIPAddress_VivekAccessToSSMS --start-ip-address 115.96.217.4 --end-ip-address 115.96.217.4
az sql server firewall-rule create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name AzureInternalAccessFirewallRule --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
az sql server firewall-rule delete --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name AzureInternalAccessFirewallRule 

To create database: 
az sql db create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name GloboTicketShoppingbasketDB --service-objective Basic
az sql db create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name GloboTicketOrderDb --service-objective Basic
az sql db create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name GloboTicketMarketingDb --service-objective Basic
az sql db create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name GloboTicketEventCatalogDb --service-objective Basic
az sql db create --resource-group Mewurk_Vivek_aks --server mewurkglobosqlserver --name GloboTicketDiscountDB --service-objective Basic

Sql Commands to fill up the databases

cd D:\Vivek\MxWork\PtMs_Globo\iac\sql
sqlcmd -S tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433 -d GloboTicketShoppingbasketDB -U mewurk_globo_admin -P vivek@123 -i Shoppingbasket-create.sql
sqlcmd -S tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433 -d GloboTicketOrderDb -U mewurk_globo_admin -P vivek@123 -i OrderDB-create.sql
sqlcmd -S tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433 -d GloboTicketMarketingDb -U mewurk_globo_admin -P vivek@123 -i MarketingDB-create.sql
sqlcmd -S tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433 -d GloboTicketEventCatalogDb -U mewurk_globo_admin -P vivek@123 -i EventCatalogDB-create.sql
sqlcmd -S tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433 -d GloboTicketDiscountDB -U mewurk_globo_admin -P vivek@123 -i TicketDiscountDB-create.sql

Service bus creation

az servicebus namespace create --resource-group Mewurk_Vivek_aks --name mewurksb --sku Standard

Once you create a new service bus, you need the get the credentials and the connection string.
https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-topics-subscriptions-portal#get-the-connection-string

kubectl create secret generic azureservicebus-secret --from-literal=az-sb-connectionstring="Endpoint=sb://mewurksb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=s/gOJH14kj1R9VHVGPQTHTFny2dvV4kNvm5Db4hNWPg="

az servicebus topic create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --name CheckoutMessage
az servicebus topic create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --name orderpaymentrequestmessage
az servicebus topic create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --name orderpaymentupdatedmessage
az servicebus topic subscription create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --topic-name CheckoutMessage --name GloboTicketOrder
az servicebus topic subscription create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --topic-name orderpaymentrequestmessage --name GloboTicketOrder
az servicebus topic subscription create --resource-group Mewurk_Vivek_aks --namespace-name mewurksb --topic-name orderpaymentupdatedmessage --name GloboTicketOrder

To delete, the service bus namespace, use the following.
az servicebus namespace delete --resource-group Mewurk_Vivek_aks --name mewurksb

If you encounter any errors regarding azure service bus, topics and subscriptions, visit the following url.
https://www.code4it.dev/blog/azure-service-bus-errors

Kubernetes Objects. Aks commands. 
az aks create --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --node-count 1 --enable-addons monitoring --generate-ssh-keys --kubernetes-version 1.20.7
az aks delete --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --yes
az aks show --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster

To scale the number of nodes, first get the name of the pool using the show command as follows.
Then use the scale command shown below it. 

az aks show --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --query agentPoolProfiles
az aks scale --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --node-count 2 --nodepool-name nodepool1

The following installs cli on the machine. Doing once per machine should be suffecient.
az aks install-cli
az aks get-credentials --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster
Now run kubectl get nodes command to show the nodes. Verify K8s
Kubectl get nodes

Kubernetese dashboard
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.3.1/aio/deploy/recommended.yaml

Take a look at the following folder on github.
https://github.com/kubernetes/dashboard/tree/master/aio/deploy/recommended
Run the following kubectl command for dashboard credentials. This I guess is obtained from the above git hub folder.
kubectl apply -f D:\Vivek\Trials\PtMs\iac\kubernetes\dashboard-credentials.yaml
kubectl proxy
Now browse to the following url
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login
To get the token, run the following in a bash shell, git bash shell for exeample.
kubectl -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep admin-user | awk '{print $1}')
Now try these for nodes and namespaces
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/namespace?namespace=default
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/node?namespace=default


kubectl get secret

kubectl create secret generic shoppingbasket-db --from-literal=connectionstring="Data Source=tcp:mewurkglobosqlserver.database.windows.net,1433;Initial Catalog=GloboTicketShoppingbasketDB;Persist Security Info=False;User ID=mewurk_globo_admin;Password=vivek@123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic ordering-db --from-literal=connectionstring="Data Source=tcp:mewurkglobosqlserver.database.windows.net,1433;Initial Catalog=GloboTicketOrderDb;Persist Security Info=False;User ID=mewurk_globo_admin;Password=vivek@123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic marketing-db --from-literal=connectionstring="Data Source=tcp:mewurkglobosqlserver.database.windows.net,1433;Initial Catalog=GloboTicketMarketingDb;Persist Security Info=False;User ID=mewurk_globo_admin;Password=vivek@123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic eventcatalog-db --from-literal=connectionstring="Data Source=tcp:mewurkglobosqlserver.database.windows.net,1433;Initial Catalog=GloboTicketEventCatalogDb;Persist Security Info=False;User ID=mewurk_globo_admin;Password=vivek@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
kubectl create secret generic discount-db --from-literal=connectionstring="Data Source=tcp:mewurkglobosqlserver.database.windows.net,1433;Initial Catalog=GloboTicketDiscountDB;Persist Security Info=False;User ID=mewurk_globo_admin;Password=vivek@123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"

kubectl create secret generic azureservicebus-secret --from-literal=az-sb-connectionstring="Endpoint=sb://mewurksb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=s/gOJH14kj1R9VHVGPQTHTFny2dvV4kNvm5Db4hNWPg="

kubectl delete secret shoppingbasket-db
kubectl delete secret ordering-db 
kubectl delete secret marketing-db 
kubectl delete secret eventcatalog-db 
kubectl delete secret discount-db

Azure Container registry
az acr create --resource-group Mewurk_Vivek_aks --name MewurkAcr --sku Basic
az acr delete --resource-group Mewurk_Vivek_aks --name MewurkAcr --yes
az acr show --resource-group Mewurk_Vivek_aks --name MewurkAcr
az acr show --resource-group Mewurk_Vivek_aks --name MewurkAcr --query loginServer 

Build and Push the repos.
cd D:\Vivek\MxWork\PtMs_Globo\iac\docker-compose
docker-compose -f docker-compose.yml -f docker-compose-build.override.yml build
Now we need to push them to the acr repo. Before pushing, we need to login. The following command will take care of that.
az acr login --name mewurkacr
docker-compose -f docker-compose.yml -f docker-compose-build.override.yml push

Create the following secret. This will enable the others to pull the images from the contianer registry.
To get the password used in the command below, go to your azure container registry, then click access keys
Now enable admin user. Note down the user name and copy the password.
kubectl create secret docker-registry acr-secret --docker-server=mewurkacr.azurecr.io --docker-username=MewurkAcr --docker-password={Your password} --docker-email=ignorethis@email.com
The final command should look something like the below.
kubectl create secret docker-registry acr-secret --docker-server=mewurkacr.azurecr.io --docker-username=MewurkAcr --docker-password=O=UzcXnPPvfx7i7RF3D0Y8WBTVT=fdI/ --docker-email=ignorethis@email.com

Now deploy the k8s objects.

cd D:\Vivek\Trials\PtMs\iac\kubernetes

kubectl apply -f aks-globoticket.gateway.mobilebff.yaml
kubectl apply -f aks-globoticket.gateway.webbff.yaml
kubectl apply -f aks-globoticket.services.discount.yaml
kubectl apply -f aks-globoticket.services.eventcatalog.yaml
kubectl apply -f aks-globoticket.services.marketing.yaml
kubectl apply -f aks-globoticket.services.ordering.yaml
kubectl apply -f aks-globoticket.services.payment.yaml
kubectl apply -f aks-globoticket.services.shoppingbasket.yaml
kubectl apply -f aks-globoticket.web.yaml

kubectl delete -f aks-globoticket.gateway.mobilebff.yaml
kubectl delete -f aks-globoticket.gateway.webbff.yaml
kubectl delete -f aks-globoticket.services.discount.yaml
kubectl delete -f aks-globoticket.services.eventcatalog.yaml
kubectl delete -f aks-globoticket.services.marketing.yaml
kubectl delete -f aks-globoticket.services.ordering.yaml
kubectl delete -f aks-globoticket.services.payment.yaml
kubectl delete -f aks-globoticket.services.shoppingbasket.yaml
kubectl delete -f aks-globoticket.web.yaml

Now launch the dashboard
kubectl proxy

Now browse to the following url
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login
Now you need a token to login. 
To get the token, run the following in a bash shell, git bash shell for exeample.
kubectl -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep admin-user | awk '{print $1}')
Now try these for nodes and namespaces
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/namespace?namespace=default
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/node?namespace=default


Scaling the Azure kubernetes cluster

az aks show --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --query agentPoolProfiles
az aks scale --resource-group Mewurk_Vivek_aks --name MewurkAksManagedCluster --node-count 2 --nodepool-name nodepool1



az ad sp create-for-rbac --skip-assignment --name MewurkAksManagedClusterServicePrincipal
az ad sp show --id f95094b5-f738-4f90-be06-ae20f3ee6b6a
az ad sp delete --id f95094b5-f738-4f90-be06-ae20f3ee6b6a

Once you create, you can see them in the azure portal as follows.
Search and go into Azure Active Directory balde.
Click on App Registrations and then All Applications or Owned Applications. 

az ad sp list --query "[?displayName=='vivek-aks-kubenet-rg']" --all
az ad sp list --query "[].{App:appDisplayName}" -o table

https://docs.microsoft.com/en-us/azure/aks/kubernetes-service-principal?tabs=azure-cli

{
  "appId": "f95094b5-f738-4f90-be06-ae20f3ee6b6a",
  "displayName": "MewurkAksManagedClusterServicePrincipal",
  "name": "f95094b5-f738-4f90-be06-ae20f3ee6b6a",
  "password": "9oFuAlPE_yE5ePr77-g7Mxg~Ck1tJhHVSe",
  "tenant": "f6b411b7-3fd3-476c-95dd-c41f67b5fc30"
}


-----------------------------------

New set of instructions

Create Secrets
kubectl create secret generic shoppingbasket-db --from-literal=connectionstring="Data Source=tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433;Initial Catalog=GloboTicketShoppingbasketDB;Persist Security Info=False;User ID=sqlserveradmin;Password=sql@Pass1234-;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic ordering-db --from-literal=connectionstring="Data Source=tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433;Initial Catalog=GloboTicketOrderDb;Persist Security Info=False;User ID=sqlserveradmin;Password=sql@Pass1234-;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic marketing-db --from-literal=connectionstring="Data Source=tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433;Initial Catalog=GloboTicketMarketingDb;Persist Security Info=False;User ID=sqlserveradmin;Password=sql@Pass1234-;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic eventcatalog-db --from-literal=connectionstring="Data Source=tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433;Initial Catalog=GloboTicketEventCatalogDb;Persist Security Info=False;User ID=sqlserveradmin;Password=sql@Pass1234-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
kubectl create secret generic discount-db --from-literal=connectionstring="Data Source=tcp:mewurk-aks-sql-server-vivek.database.windows.net,1433;Initial Catalog=GloboTicketDiscountDB;Persist Security Info=False;User ID=sqlserveradmin;Password=sql@Pass1234-;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False;"
kubectl create secret generic azureservicebus-secret --from-literal=az-sb-connectionstring="Endpoint=sb://mewurkgloboservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=v3iekK4uovy/Hkang8wf/hPlYdn8kvjq+LkP6ivWI9g="
kubectl create secret docker-registry acr-secret --docker-server=mewurkacr.azurecr.io --docker-username=mewurkacr --docker-password=o=jCCkWEtgHW6AjYbYzLtI2PojLorPF2 --docker-email=ignorethis@email.com

Now launch the dashboard
kubectl proxy

Now browse to the following url
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login
Now you need a token to login. 
To get the token, run the following in a bash shell, git bash shell for exeample.
kubectl -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep admin-user | awk '{print $1}')
Now try these for nodes and namespaces
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/namespace?namespace=default
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/node?namespace=default


Build and Push the repos.
cd D:\Vivek\MxWork\PtMs_Globo\iac\docker-compose
docker-compose -f docker-compose.yml -f docker-compose-build.override.yml build
Now we need to push them to the acr repo. Before pushing, we need to login. The following command will take care of that.
az acr login --name mewurkacr
docker-compose -f docker-compose.yml -f docker-compose-build.override.yml push

cd D:\Vivek\MxWork\PtMs_Globo\iac\kubernetes
kubectl apply -f aks-globoticket.services.discount.yaml
kubectl apply -f aks-globoticket.services.eventcatalog.yaml
kubectl apply -f aks-globoticket.services.ordering.yaml
kubectl apply -f aks-globoticket.services.shoppingbasket.yaml
kubectl apply -f aks-globoticket.web.yaml

kubectl delete -f aks-globoticket.services.discount.yaml
kubectl delete -f aks-globoticket.services.eventcatalog.yaml
kubectl delete -f aks-globoticket.services.ordering.yaml
kubectl delete -f aks-globoticket.services.shoppingbasket.yaml
kubectl delete -f aks-globoticket.web.yaml



az aks update --resource-group MeWurk_Vivek_aks_Rg --name mewurk-aks-aks-vivek --attach-acr mewurkacr
az acr import  -n mewurkacr --source docker.io/library/nginx:latest --image nginx:v1
az aks check-acr --name mewurk-aks-aks-vivek --resource-group MeWurk_Vivek_aks_Rg --acr mewurkacr.azurecr.io
az aks get-credentials --name mewurk-aks-aks-vivek --resource-group MeWurk_Vivek_aks_Rg

https://docs.microsoft.com/en-us/azure/aks/kubernetes-service-principal?tabs=azure-cli
https://docs.microsoft.com/en-us/azure/aks/cluster-container-registry-integration?tabs=azure-cli
https://docs.microsoft.com/en-us/cli/azure/aks?view=azure-cli-latest#az_aks_check_acr

The following Service Connections in azure devops needs to be recreated.
mewurk-globoticket-acr-service-connection
mewurk-globoticket-aks-service-connection