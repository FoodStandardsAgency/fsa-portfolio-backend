﻿DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'FSAPortfolio.Entites.Migrations.Configuration'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'

IF @CurrentMigration < '202009302115568_Create'
BEGIN
    CREATE TABLE [dbo].[AccessGroups] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](50),
        CONSTRAINT [PK_dbo.AccessGroups] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[ProjectOnHoldStatus] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        CONSTRAINT [PK_dbo.ProjectOnHoldStatus] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[ProjectPhases] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        CONSTRAINT [PK_dbo.ProjectPhases] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[ProjectRAGStatus] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        CONSTRAINT [PK_dbo.ProjectRAGStatus] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[Projects] (
        [Id] [int] NOT NULL IDENTITY,
        [ProjectId] [nvarchar](10),
        [Name] [nvarchar](250),
        [Description] [nvarchar](1000),
        [Priority] [int] NOT NULL,
        [StartDate] [datetime],
        [ActualStartDate] [datetime],
        [ExpectedEndDate] [datetime],
        [HardEndDate] [datetime],
        [LatestUpdate_Id] [int],
        CONSTRAINT [PK_dbo.Projects] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_LatestUpdate_Id] ON [dbo].[Projects]([LatestUpdate_Id])
    CREATE TABLE [dbo].[ProjectUpdateItems] (
        [Id] [int] NOT NULL IDENTITY,
        [Project_Id] [int] NOT NULL,
        [Timestamp] [datetime] NOT NULL,
        [Text] [nvarchar](max),
        [SyncId] [int] NOT NULL,
        [OnHoldStatus_Id] [int],
        [Person_Id] [int],
        [Phase_Id] [int],
        [RAGStatus_Id] [int],
        CONSTRAINT [PK_dbo.ProjectUpdateItems] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_Project_Id] ON [dbo].[ProjectUpdateItems]([Project_Id])
    CREATE INDEX [IX_OnHoldStatus_Id] ON [dbo].[ProjectUpdateItems]([OnHoldStatus_Id])
    CREATE INDEX [IX_Person_Id] ON [dbo].[ProjectUpdateItems]([Person_Id])
    CREATE INDEX [IX_Phase_Id] ON [dbo].[ProjectUpdateItems]([Phase_Id])
    CREATE INDEX [IX_RAGStatus_Id] ON [dbo].[ProjectUpdateItems]([RAGStatus_Id])
    CREATE TABLE [dbo].[People] (
        [Id] [int] NOT NULL IDENTITY,
        CONSTRAINT [PK_dbo.People] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[Users] (
        [Id] [int] NOT NULL IDENTITY,
        [Timestamp] [datetime] NOT NULL,
        [UserName] [nvarchar](50),
        [PasswordHash] [nvarchar](300),
        [AccessGroupId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Users] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_AccessGroupId] ON [dbo].[Users]([AccessGroupId])
    ALTER TABLE [dbo].[Projects] ADD CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_LatestUpdate_Id] FOREIGN KEY ([LatestUpdate_Id]) REFERENCES [dbo].[ProjectUpdateItems] ([Id])
    ALTER TABLE [dbo].[ProjectUpdateItems] ADD CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectOnHoldStatus_OnHoldStatus_Id] FOREIGN KEY ([OnHoldStatus_Id]) REFERENCES [dbo].[ProjectOnHoldStatus] ([Id])
    ALTER TABLE [dbo].[ProjectUpdateItems] ADD CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.People_Person_Id] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[People] ([Id])
    ALTER TABLE [dbo].[ProjectUpdateItems] ADD CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectPhases_Phase_Id] FOREIGN KEY ([Phase_Id]) REFERENCES [dbo].[ProjectPhases] ([Id])
    ALTER TABLE [dbo].[ProjectUpdateItems] ADD CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectRAGStatus_RAGStatus_Id] FOREIGN KEY ([RAGStatus_Id]) REFERENCES [dbo].[ProjectRAGStatus] ([Id])
    ALTER TABLE [dbo].[ProjectUpdateItems] ADD CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([Id])
    ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.AccessGroups_AccessGroupId] FOREIGN KEY ([AccessGroupId]) REFERENCES [dbo].[AccessGroups] ([Id])
    CREATE TABLE [dbo].[__MigrationHistory] (
        [MigrationId] [nvarchar](150) NOT NULL,
        [ContextKey] [nvarchar](300) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
    )
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'202009302115568_Create', N'FSAPortfolio.Entites.Migrations.Configuration',  0x1F8B0800000000000400ED1DDB6EDCBAF1BD40FF41D0535BF8ECFA8202A7C6EE39701D3B311AC70BAF73D0378396B86BB5BA55E2BA368AF3657DE827F5174AEACABB48AD64AB891120B125CE8533C3D10C879CFCF7DFFF59FCFC1C85CE13CCF2208997EED1ECD07560EC257E106F97EE0E6D7EF8D1FDF9A7DFFE6671E147CFCE2FF5B813320E43C6F9D27D44283D9DCF73EF1146209F4581972579B241332F89E6C04FE6C787877F9A1F1DCD2146E1625C8EB3B8DDC5288860F10BFEF53C893D98A21D08AF131F8679F51CBF5917589D2F2082790A3CB8742FD767AB24439B240C92D905468360EE3A676100302F6B186E5C07C4718200C29C9E7ECDE11A6549BC5DA7F80108EF5E5288C76D4098C36A06A7ED70D3C91C1E93C9CC5BC01A95B7CB511259223C3AA9A433E7C17BC9D86DA487E55708E885CCBA90E1D23DF33C98E71FB36497BA0E4FEFF43CCCC858B994675898593EA3301C38B271078D9D6073227F0E9CF35D8876195CC6708732101E38ABDD4318787F812F77C9DF61BC8C776148F38D39C7EF9807F8D12A4B5298A1975BB8A96673E5BBCE9C859BF3800D1805534EF22A4627C7AEF30513070F216CCC8212C81A2519FC0863980104FD1540086631C1010BC10AD4395AE4EF9A1AB643BCA85CE71A3C7F86F1163D2EDD3FE25574193C43BF7E5031F0350EF012C43028DB4196C662DEAA54AB68CCC9DFA0876EE24F49E8AFF17476B9B5C22B1CCD0F34B277DDEFA77BFCE3E8CA5F3D0282714FAD1758DED53D7D75DF9E7D1C68A13798DED53E7DB5EFADED77257728B992534B52A2E923FB6FB9B5351DF7081844221F60EE65415A06989AF91C0E416C950549568858AF2C3D16EC8C32F401EBAE46437EBE0BA24EC0338F6415BDC12F9E53AC77E85FC47E1FF04F20EB0BFA198FCBD1D7D4C7FFDE0BB6CEC17E014FC1B6B0740D16D7B9856131287F0CD23257AA3DC03D3BF0324BA2DB246C2D9F797FBF4E76994766946806DD816C0B9139AB2558AEE5B219233258BD52F256BF97B165EB704B5C5708467BBBDE16D5BB133673C2E262B0F42564F5E50844A9B8246D31C16734F0B75FE2FA5E62CF7ACACA55C6A683EAA5D6DAE53D0B22AC3CC548D542540DB775172BBC2CC8F7CB680AF5601DF3E51803B6AB81D60C97B99819BFE5582DBB648809B7C5386B66EBC8723F675C935539E39A7D53B6A804C7488ED4789D2C9B6106F26CC7EEF725A92CB2D70E5C09FCFEC1E8277A22C39E82277FBF8BFDD5BEAF44DCC3EFA04A220B90E7FF4C32FF13C81F35B44E064988A8EDF3E13EEBCCAEBEC4351249DE33835A7FC8BF139CA030C0C6F39DE579E205053FAA209A8D30D819E3ECC9B10B374AED49BF07589B784D06295E8598D3A5FB0741C0C6E49A2F9B408E0D97588287B3D9112F364A40B672AB3E22E653E0639CD164C5C54834A1EAC337AE608AF8C9825D36981A4F2C4C3026D2A9E2BE5165D3C644E67C4B02A4D164240658222D2AAE1B5E56EC5E4807E38A8D1181E31E32916FA7D8085E940629BDC30DCCC8971C84E7F81B814393204662D411C45E9082D080250ED63066219A69A8F06F3EC014C624DC3010B6097961334DE4A521C945565DE2EA615F75E2D4A57F614BABD3AA446DABB1F63128D16AF7B2266E82AF68489C104C28D3DB4F6F623E420CA7D2B43AA06B555D2641E63E491903523899F8B2CB307B198E8A8B57B01C95504D487321FF2B584F198B6318842160569B709DB59217C5DEA59005E3695689705EA533BC4510C46B88C4A9E18F719B02C84C42302E1695247E86329CD238DB0C77115F697056F19719B22608D161A4221533AC1A5CA6285AFFAD41463BF92EB4304943284355C5F21DF0C5DE8904BCF4411C3065D09A89719BD21490D93636BF02ADD3CC668E72A90B4BDC3AB11409F00B83DF036225D74BAAB5424DE4294B3EEDD2CF2165C8259C34EACA7AC71057E92E8CA42566A45639E9A0B262B2501173ED25479018E50E4DA4A6C855ADB3D521A527E6A72276E6D3309418B91ABD527E9AFCD53C8315E764242779CE6A25FE3D04D454A4D4B291E65E46D9575F89F0F9D6D8C210B77A4569E85309B364829A47F581D70843993E5058D850D2580EF5C67313EC36EF16F3F2F640F56031575C33585C83340DE22D75EDA07AE2ACCB3B07E73FACED8FE247258EB9974B4EE437DC36945092812DE4DE62D298D3CB20CBC9F125F0507C6FCEFD48182684F68A90AC262789DE45EDD5F15A0D447E2E01A535328D41B522BDC4B38C485A555499E4EA17A11D721304842093D4B5CE937017C5EA244F0D5DD69668F8F289886131E7D8175238415A9CFDF21A30D28F3CF2DB5B51B20CCA5E614658BE6FC55591D4500A930691E69A52807FDF2AA243B5A1F4A40C5BCD75A541F17DEB6B382DF557CE6BE964D51E3BA791508FC7D0AF0A03736C9C46C4BCB0995D7D329C9D5CFDD41C1375BC9B46453D36C7251C18A7310A2FCDF10A27C969BCC24B73BCCC11731A27F3C21C9F502AA37176D6D1DEDA3BD0A9D4507E82DA29EDED31743846F51D820A75652C3536EA4C178D8C7A6C81ABA83C30688A2716EBBD3A12CD2CF6EAD9742CB2DC77DCDF0AA5FBAC26A6A700DCCFDEDE489AE516C3DEC22C4A0EF6A294838DB370875C6AED014A1A55FBD4C29D3047241987C2BCB1F9D0324551F633ABAD978E6F84EC6653C79786AB45D97E543870EB0F08D96593FBFE8E12932855236B6650C9CBD914477B33AB2CB3F75E7A5AFECE93D80F8A43175739398DDB9CC4B51001BF59398089D585396BE3AA01C7312BB6EAD653471592514C89AD384ECA8894D31EC37CCA42A5BDF5947023190F5D86EC6B3B058E714C87AEBF4ECB7214931EC170A87AADB5F150B0E318905089EDA9A516CF28862494A327654CBAC9771B94507CE38734915A5384E38A6D8BAAF0D5DDF84BA88495435C07CFFF29F049156CFD926389CFC880D9FA1FE17918E0D8B01D700DE26083A3E8F2FE957B7C7874CC750E9B4E17AF799EFBCCF530DB565E57B10F9F97EEBF9C5F5FFF925940A4DE798DCCF2365899B29404E22790798F2013EE82B538076E9DF5EDCAF0771178FEFDB09293DDBF7917995664AA6B39EF62D38AED7B707C54BD47E3FD8E3ABC5F0F877ADCE551459C92DE420A6E0FAD71F3AD840A61EFDB4888846988BB9C6C768D57DA58A82F3A45A3A1BEE8248D87FAA25234222A84AF5E7B04F6D4B9FAEB3D077EE0DC6438123B750EF1F21CD413689BF27CAB3EA187425A48952EFA75D191D99775171D8B2F85646D339D72FA78076163CF4EB41CB8B9AD4BF4DBEE0E59AAB706DC8B7AB3C36049BC82DB87369B90DAD1A761C7F034B2FBF6FF1F1E64DFEE28DF9C031DCE7FF15D4A7AE5A6923528694A22457D621F49497B90982E3206D8E21BF25A1D42C472DC3E0D3A6414E850E3EDAE4F771CDF3559854E8FBBD3DAE33022518372E1EB5DBDB7E89D4215DC2C3B974CD162546748266224DA12E0DB9887490719AAACD6AB8FCB142D457FD27C2AF6A229FBBD89B91837D5E10A6ABDDBDA4CD7747487DF27623E5D05BF57EC0563D36BA8AF7ECDACA75FD7A27D2CC5526BC398CAB7DB2F68A00641D3F022D3751EF4CEDB9B9885711F20A63EDCB74F0F7B87B7470FA15E56A0BF4A3AB825288E058B74A6D2C947BC15CC6B4DD9A6A7AB4B4F79AA62E9FA0F09D67699CD6BAF66F3B4A469B640533A4A465B3AD08C852A0C57D1AE4FB7A9892A3A6128C851E19B8A247D264A4DD6BA6990AE6790868E317EDAEBAB28D163343475CD0E04EA55C62D92AC4FB5CAE8C8FBBDF0B84BE725602E1FCBF04A9B1C4CA55D916CB1292F695BF53BD137AC18A549CC6AD02E448C0DD11763262F86C1BB0B31FE50BC193E75818CD73C48F0DCF2ABD89314D0486D81FA4E449E9948EE500F278021DBFE98B0FAC67A1FB8C18F243414FAB1E804407F48DBCB72E6D3B4E8DF231E15C68135F53F09E3B83E0FB62D0A72023AC6E2A743EA66CC55BC49EA009FE3A81EC255CDAE2102588FE02C43C10660AF99254444C57F13F00B087778C845F400FDABF86687D21DC25386D143C85C652719828E7ED1A488E57971531CA7CA879802663320A67813FF7917847EC3F7A5A45CA74041528FAA0E4B7489483D76FBD260FA92C486882AF13519D31D8CD290ACD29B780D9E601FDEB0E97D865BE0BDACAA13DF6A24DD8A60C5BEF810806D06A2BCC2D1C2E35FB10DFBD1F34FFF03B92C3997507B0000 , N'6.4.4')
END

