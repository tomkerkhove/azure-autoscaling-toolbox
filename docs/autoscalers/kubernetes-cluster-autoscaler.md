---
layout: default
title: Cluster Autoscaler for Azure Kubernetes Service
parent: Autoscalers
---

# Cluster Autoscaler for Azure Kubernetes Service

[Docs](https://docs.microsoft.com/en-us/azure/aks/cluster-autoscaler){: .btn }
[Best Practices](**https://docs.microsoft.com/en-us/azure/azure-monitor/service-limits#autoscale**){: .btn }
[Limitations](https://docs.microsoft.com/en-us/azure/azure-monitor/service-limits#autoscale){: .btn }

**Cluster Autoscaler for Azure Kubernetes Service** automatically scales a node pool in your Azure Kubernetes Service cluster to meet the cluster demand.

This is a managed offering of the open-source [Kubernetes Cluster Autoscaler](https://github.com/kubernetes/autoscaler/tree/master/cluster-autoscaler) so that you have to worry less about the infrastructure.

## Maintainer

Kubernetes Community / CNCF.

## Scaling Mechanism

Scale out or in by adding or removing instances.

## Deployment

Available as a service and managed through ARM, REST API, Azure Portal, or Azure CLI.

## Support

Yes, through Azure Support.

## Observability

Azure Kubernetes Service provides a variety of ways to gain insights on your autoscaling:

- [Diagnostic Logs](https://docs.microsoft.com/en-us/azure/aks/cluster-autoscaler#retrieve-cluster-autoscaler-logs-and-status)
- Kubernetes Events ([Kubernetes docs](https://github.com/kubernetes/autoscaler/blob/master/cluster-autoscaler/FAQ.md#what-events-are-emitted-by-ca))
    - You can forward them to Azure Event Grid by using [Kubernetes Event Grid Bridge](https://docs.k8s-event-grid-bridge.io/)
- Metrics (Preview)

## Supported Services

You can autoscale the following Azure services:

- Azure Kubernetes Service

## Limitations

- Cluster Autoscaler will be in your Kubernetes cluster and needs to be operated.
