---
layout: default
title: Kubernetes Event-Driven Autoscaling (KEDA)
parent: Autoscalers
---

# Kubernetes Event-Driven Autoscaling (KEDA)

[Docs](https://keda.sh/){: .btn }
[GitHub](https://github.com/kedacore/keda){: .btn }
[Sample](https://github.com/kedacore/sample-dotnet-worker-servicebus-queue){: .btn }

**Kubernetes Event-Driven Autoscaling (KEDA)** is a Kubernetes-based Event Driven Autoscaler that makes application autoscaling dead-simple. With KEDA, you can drive the scaling of any workload in Kubernetes based on the number of events needing to be processed.

## Maintainer

CNCF-based, but here's an overview of the [active maintainers](https://github.com/kedacore/governance/blob/main/MAINTAINERS.md).

## Scaling Mechanism

Scale out or in by adding or removing instances.

## Deployment

Easy to install in your Kubernetes cluster through [Helm](https://keda.sh/docs/latest/deploy/#helm) or [OperatorHub](https://keda.sh/docs/latest/deploy/#operatorhub).

> 💡 *A [feature request](https://github.com/Azure/AKS/issues/1479) is open to have KEDA as an add-on for Azure Kubernetes Service.*

## Support

Community-based support is available, learn more [here](https://github.com/kedacore/governance/blob/main/SUPPORT.md).

## Observability

Kubernetes Event-Driven Autoscaling (KEDA) provides a variety of ways to gain insights on your autoscaling:

- [Kubernetes Events](https://keda.sh/docs/latest/operate/events/)
    - You can forward them to Azure Event Grid by using [Kubernetes Event Grid Bridge](https://docs.k8s-event-grid-bridge.io/)

## Supported Services

[Full Overview](https://keda.sh/docs/latest/scalers/){: .btn }

You can autoscale based on the following Azure services:

- Azure Event Hubs
- Azure Log Analytics
- Azure Monitor
- Azure Pipelines
- Azure Service Bus
- Azure Storage (Blob)
- Azure Storage (Queue)

## Limitations

- You have to deploy and operate KEDA in your Kubernetes cluster.
