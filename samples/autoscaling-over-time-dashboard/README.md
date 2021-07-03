# Autoscaling-over-time Dashboard

Leverage scaling-over-time dashboarding for your application autoscaling with Azure Durable Functions. 📊

![](../../docs/media/logo/logo-with-name.png)

## What does it provide?

Use `App Instances` metrics to visualise how the applications in your platform are scaling.

![](./media/general-overview.png)

## Gain deeper insights with dimensions

Want to have more details? Use the various dimensions that are provided:

- For Kubernetes applications, we provide:
  - `AppName` - Name of the deployment in the cluster
  - `Namespace` - Name of the Kubernetes namespace
  - `Runtime` - Fixed to Kubernets

- For generic applications scaled with Azure Monitor, we provide:
  - `AppName` - Name of the Azure resource
  - `SubscriptionId` - Name of the Azure subscription
  - `ResourceGroup` - Name of the Azure resource group
  - `Region` - Name of the Azure region
  - `Runtime` - Name or type of Azure service hosting your application, for example `Azure App Service`

Here are a few examples:

<details>
  <summary>Instances per application</summary>
  
![](./media/overview-per-application.png)

</details>

<details>
  <summary>Application Instances per runtime</summary>
  
![](./media/overview-per-runtime.png)

</details>

<details>
  <summary>Application Instances per Kubernetes namespace</summary>
  
![](./media/overview-per-kubernetes-namespace.png)

</details>

## How does it work?

For every application in your platform, a durable entity is available that allows you to:

- Receive CloudEvents for Azure Monitor Autoscale events (based on [Azure Event Grid adapter for Azure Monitor Autoscale](https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter))
- Receive CloudEvents for Kubernetes application events (based on [Kubernetes Event Grid Bridge]([https://docs.k8](https://docs.k8s-event-grid-bridge.io/)))
- Automatically report current instance count every 5 minutes
- Get the current instance count for a given app

Here's a high-level overview:

![](../../docs/media/samples/autoscaling-over-time-dashboard.png)

## API Overview

Here is an overview of the APIs you can interact with.

### Kubernetes Scaling Event

TODO

### Azure Monitor Scaling Event

TODO

### Get Instance Count for Kubernetes App

TODO

### Get Instance Count for Generic App

TODO