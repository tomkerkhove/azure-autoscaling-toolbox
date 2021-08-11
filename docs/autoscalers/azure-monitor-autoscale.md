---
layout: default
title: Azure Monitor Autoscale
parent: Autoscalers
---

# Azure Monitor Autoscale

[Docs](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-overview){: .btn }
[Get Started](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-get-started){: .btn }
[Common Patterns](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-common-scale-patterns){: .btn }
[Best Practices](https://docs.microsoft.com/en-us/azure/azure-monitor/service-limits#autoscale){: .btn }
[Limitations](https://docs.microsoft.com/en-us/azure/azure-monitor/service-limits#autoscale){: .btn }

**Azure Monitor Autoscale is an autoscaler-as-a-service** that allows you to automatically scale a variety of Azure resources in/out based on the scaling criteria that you define.

The scaling criteria allows you to define when to scale out and when to scale in, based on metrics or a schedule. The rules that are defined can also control how many instances should be added/removed and how long it should wait before taking any other actions.

Learn more how to get started in the [documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-get-started).

## Maintainer

Microsoft

## Scaling Mechanism

Scale out or in by adding or removing instances.

## Deployment

Available as a service and managed through ARM, REST API, Azure Portal, or Azure CLI.

## Support

Yes, through Azure Support.

## Observability

Azure Monitor Autoscale provides a variety of ways to gain insights on your autoscaling:

- [Receive Webhooks & Emails](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-webhook-email)
- [Visualize Metrics](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-troubleshoot#autoscale-metrics)
- [View Autoscaling Events](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-troubleshoot#example-3---understanding-autoscale-events)
- [View Autoscaling Logs](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-troubleshoot#autoscale-resource-logs)

## Supported Services

[Full Overview](https://docs.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-overview#supported-services-for-autoscale){: .btn }

You can autoscale the following Azure services:

- Azure API Management
- Azure App Service
- Azure Cloud Services
- Azure Data Explorer Clusters
- Azure Logic Apps
- Azure Service Bus
- Azure Spring Cloud
- Azure Virtual Machines
- Azure Virtual Machines Scale Sets
- Azure Web Apps

## Limitations

Learn more about the limitations in the [documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/service-limits#autoscale).
