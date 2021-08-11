---
layout: page
title: Observability
permalink: /observability/
nav_order: 4
---

# Observability

Autoscaling is one thing, understanding how you are scaling is another.

Standardizing on Azure Event Grid is a great approach to centralize your autoscaling notifications:

![](/media/observability/event-grid-as-autoscaling-hub.png)

Here are a few ways to integrate with Azure Monitor for your autoscaling:

- **Kubernetes Event Grid Bridge** - A simple event bridge for Kubernetes native events to Azure Event Grid. ([GitHub](https://github.com/tomkerkhove/k8s-event-grid-bridge) - [Docs](https://docs.k8s-event-grid-bridge.io/) - [Blog post](https://blog.tomkerkhove.be/2021/01/19/introducing-kubernetes-event-grid-bridge/))
- **Azure Event Grid adapter for Azure Monitor Autoscale** - Bring Azure Monitor Autoscale events to Azure Event Grid with this adapter. ([GitHub](https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter) - [Blog post](https://blog.tomkerkhove.be/2021/02/10/automatically-forwarding-azure-monitor-autoscale-events-to-azure-event-grid/))