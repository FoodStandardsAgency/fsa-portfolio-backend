USE [Portfolio]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_dbo.Users_dbo.AccessGroups_AccessGroupId]
GO
ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_dbo.Teams_dbo.Portfolios_Portfolio_Id]
GO
ALTER TABLE [dbo].[RelatedProjects] DROP CONSTRAINT [FK_dbo.RelatedProjects_dbo.Projects_RelatedProject_Id]
GO
ALTER TABLE [dbo].[RelatedProjects] DROP CONSTRAINT [FK_dbo.RelatedProjects_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectUpdateItems] DROP CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectUpdateItems] DROP CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectRAGStatus_RAGStatus_Id]
GO
ALTER TABLE [dbo].[ProjectUpdateItems] DROP CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectPhases_Phase_Id]
GO
ALTER TABLE [dbo].[ProjectUpdateItems] DROP CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectOnHoldStatus_OnHoldStatus_Id]
GO
ALTER TABLE [dbo].[ProjectUpdateItems] DROP CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.People_Person_Id]
GO
ALTER TABLE [dbo].[ProjectTeamMembers] DROP CONSTRAINT [FK_dbo.ProjectTeamMembers_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectTeamMembers] DROP CONSTRAINT [FK_dbo.ProjectTeamMembers_dbo.People_Person_Id]
GO
ALTER TABLE [dbo].[ProjectSubcategories] DROP CONSTRAINT [FK_dbo.ProjectSubcategories_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectSubcategories] DROP CONSTRAINT [FK_dbo.ProjectSubcategories_dbo.ProjectCategories_Subcategory_Id]
GO
ALTER TABLE [dbo].[ProjectSizes] DROP CONSTRAINT [FK_dbo.ProjectSizes_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_LatestUpdate_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_FirstUpdate_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.ProjectSizes_ProjectSize_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.ProjectReservations_ProjectReservation_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.ProjectCategories_ProjectCategory_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.People_ServiceLead_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.People_Lead_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact3_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact2_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact1_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.Directorates_Directorate_Id]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_dbo.Projects_dbo.BudgetTypes_BudgetType_Id]
GO
ALTER TABLE [dbo].[ProjectReservations] DROP CONSTRAINT [FK_dbo.ProjectReservations_dbo.Portfolios_Portfolio_Id]
GO
ALTER TABLE [dbo].[ProjectRAGStatus] DROP CONSTRAINT [FK_dbo.ProjectRAGStatus_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[ProjectPhases] DROP CONSTRAINT [FK_dbo.ProjectPhases_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[ProjectOnHoldStatus] DROP CONSTRAINT [FK_dbo.ProjectOnHoldStatus_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[ProjectDocuments] DROP CONSTRAINT [FK_dbo.ProjectDocuments_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectDocuments] DROP CONSTRAINT [FK_dbo.ProjectDocuments_dbo.Documents_Document_Id]
GO
ALTER TABLE [dbo].[ProjectDataItems] DROP CONSTRAINT [FK_dbo.ProjectDataItems_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[ProjectDataItems] DROP CONSTRAINT [FK_dbo.ProjectDataItems_dbo.PortfolioLabelConfigs_Label_Id]
GO
ALTER TABLE [dbo].[ProjectCategories] DROP CONSTRAINT [FK_dbo.ProjectCategories_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[ProjectAuditLogs] DROP CONSTRAINT [FK_dbo.ProjectAuditLogs_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[PortfolioProjects] DROP CONSTRAINT [FK_dbo.PortfolioProjects_dbo.Projects_Project_ProjectReservation_Id]
GO
ALTER TABLE [dbo].[PortfolioProjects] DROP CONSTRAINT [FK_dbo.PortfolioProjects_dbo.Portfolios_Portfolio_Id]
GO
ALTER TABLE [dbo].[PortfolioLabelGroups] DROP CONSTRAINT [FK_dbo.PortfolioLabelGroups_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[PortfolioLabelConfigs] DROP CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioLabelGroups_Group_Id]
GO
ALTER TABLE [dbo].[PortfolioLabelConfigs] DROP CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioLabelConfigs_MasterLabel_Id]
GO
ALTER TABLE [dbo].[PortfolioLabelConfigs] DROP CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioConfigurations_Configuration_Id]
GO
ALTER TABLE [dbo].[PortfolioConfigurations] DROP CONSTRAINT [FK_dbo.PortfolioConfigurations_dbo.ProjectPhases_CompletedPhase_Id]
GO
ALTER TABLE [dbo].[PortfolioConfigurations] DROP CONSTRAINT [FK_dbo.PortfolioConfigurations_dbo.Portfolios_Portfolio_Id]
GO
ALTER TABLE [dbo].[PortfolioConfigAuditLogs] DROP CONSTRAINT [FK_dbo.PortfolioConfigAuditLogs_dbo.PortfolioConfigurations_PortfolioConfiguration_Id]
GO
ALTER TABLE [dbo].[People] DROP CONSTRAINT [FK_dbo.People_dbo.Teams_Team_Id]
GO
ALTER TABLE [dbo].[DependantProjects] DROP CONSTRAINT [FK_dbo.DependantProjects_dbo.Projects_Project_Id]
GO
ALTER TABLE [dbo].[DependantProjects] DROP CONSTRAINT [FK_dbo.DependantProjects_dbo.Projects_DependantProject_Id]
GO
ALTER TABLE [dbo].[BudgetTypes] DROP CONSTRAINT [FK_dbo.BudgetTypes_dbo.PortfolioConfigurations_Configuration_Id]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[Teams]
GO
/****** Object:  Table [dbo].[RelatedProjects]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[RelatedProjects]
GO
/****** Object:  Table [dbo].[ProjectUpdateItems]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectUpdateItems]
GO
/****** Object:  Table [dbo].[ProjectTeamMembers]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectTeamMembers]
GO
/****** Object:  Table [dbo].[ProjectSubcategories]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectSubcategories]
GO
/****** Object:  Table [dbo].[ProjectSizes]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectSizes]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[Projects]
GO
/****** Object:  Table [dbo].[ProjectReservations]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectReservations]
GO
/****** Object:  Table [dbo].[ProjectRAGStatus]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectRAGStatus]
GO
/****** Object:  Table [dbo].[ProjectPhases]    Script Date: 12/11/2020 12:45:40 ******/
DROP TABLE [dbo].[ProjectPhases]
GO
/****** Object:  Table [dbo].[ProjectOnHoldStatus]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[ProjectOnHoldStatus]
GO
/****** Object:  Table [dbo].[ProjectDocuments]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[ProjectDocuments]
GO
/****** Object:  Table [dbo].[ProjectDataItems]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[ProjectDataItems]
GO
/****** Object:  Table [dbo].[ProjectCategories]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[ProjectCategories]
GO
/****** Object:  Table [dbo].[ProjectAuditLogs]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[ProjectAuditLogs]
GO
/****** Object:  Table [dbo].[Portfolios]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[Portfolios]
GO
/****** Object:  Table [dbo].[PortfolioProjects]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[PortfolioProjects]
GO
/****** Object:  Table [dbo].[PortfolioLabelGroups]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[PortfolioLabelGroups]
GO
/****** Object:  Table [dbo].[PortfolioLabelConfigs]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[PortfolioLabelConfigs]
GO
/****** Object:  Table [dbo].[PortfolioConfigurations]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[PortfolioConfigurations]
GO
/****** Object:  Table [dbo].[PortfolioConfigAuditLogs]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[PortfolioConfigAuditLogs]
GO
/****** Object:  Table [dbo].[People]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[People]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[Documents]
GO
/****** Object:  Table [dbo].[Directorates]    Script Date: 12/11/2020 12:45:41 ******/
DROP TABLE [dbo].[Directorates]
GO
/****** Object:  Table [dbo].[DependantProjects]    Script Date: 12/11/2020 12:45:42 ******/
DROP TABLE [dbo].[DependantProjects]
GO
/****** Object:  Table [dbo].[BudgetTypes]    Script Date: 12/11/2020 12:45:42 ******/
DROP TABLE [dbo].[BudgetTypes]
GO
/****** Object:  Table [dbo].[AccessGroups]    Script Date: 12/11/2020 12:45:42 ******/
DROP TABLE [dbo].[AccessGroups]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 12/11/2020 12:45:42 ******/
DROP TABLE [dbo].[__MigrationHistory]
GO
