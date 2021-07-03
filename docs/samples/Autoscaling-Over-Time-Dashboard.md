---
layout: default
title: Autoscaling Over Time Dashboard
parent: Samples
redirect_from:
  - /samples/Stateful-App-Instances.html
---

## Autoscaling Over Time Dashboard

[GitHub](https://github.com/tomkerkhove/azure-autoscaling-toolbox/tree/main/samples/autoscaling-over-time-dashboard){: .btn }

Leverage scaling-over-time dashboarding for your application autoscaling with Azure Durable Functions. 📊

![](../media/samples/autoscaling-over-time-dashboard/general-overview.png)

Here are a few more examples:

<details>
  <summary>Instances per application</summary>

<img src="../media/samples/autoscaling-over-time-dashboard/overview-per-application.png" />
</details>

<details>
  <summary>Application Instances per runtime</summary>

<img src="../media/samples/autoscaling-over-time-dashboard/overview-per-runtime.png" />

</details>

<details>
  <summary>Application Instances per Kubernetes namespace</summary>

<img src="../media/samples/autoscaling-over-time-dashboard/overview-per-kubernetes-namespace.png" />

</details>

## How does it work?

For every application in your platform, a durable entity is available that allows you to:

- Receive CloudEvents for Azure Monitor Autoscale events (based on [Azure Event Grid adapter for Azure Monitor Autoscale](https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter))
- Receive CloudEvents for Kubernetes application events (based on [Kubernetes Event Grid Bridge]([https://docs.k8](https://docs.k8s-event-grid-bridge.io/)))
- Automatically report current instance count every 5 minutes
- Get the current instance count for a given app

Here's a high-level overview:

![](../media/samples/autoscaling-over-time-dashboard/how-it-works.png)
