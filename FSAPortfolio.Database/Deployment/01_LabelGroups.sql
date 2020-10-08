﻿CREATE TABLE [dbo].[PortfolioLabelGroups] (
    [Id] [int] NOT NULL IDENTITY,
    [Configuration_Id] [int] NOT NULL,
    [Name] [nvarchar](50),
    [Order] [int] NOT NULL,
    CONSTRAINT [PK_dbo.PortfolioLabelGroups] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Configuration_Id] ON [dbo].[PortfolioLabelGroups]([Configuration_Id])
ALTER TABLE [dbo].[PortfolioLabelConfigs] ADD [Group_Id] [int]
CREATE INDEX [IX_Group_Id] ON [dbo].[PortfolioLabelConfigs]([Group_Id])
ALTER TABLE [dbo].[PortfolioLabelConfigs] ADD CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioLabelGroups_Group_Id] FOREIGN KEY ([Group_Id]) REFERENCES [dbo].[PortfolioLabelGroups] ([Id])
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.PortfolioLabelConfigs')
AND col_name(parent_object_id, parent_column_id) = 'FieldGroup';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[PortfolioLabelConfigs] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[PortfolioLabelConfigs] DROP COLUMN [FieldGroup]
ALTER TABLE [dbo].[PortfolioLabelGroups] ADD CONSTRAINT [FK_dbo.PortfolioLabelGroups_dbo.PortfolioConfigurations_Configuration_Id] FOREIGN KEY ([Configuration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Id])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202010081015485_01_LabelGroups', N'FSAPortfolio.Entities.Migrations.Configuration',  0x1F8B0800000000000400ED5D5B8FDCB8727E0F90FFD0E8A7E4C0677AC63EBBD93566CE81D7B7358EBD1EB8BD8BBC197237674659B5D447527B3D09F2CBF2909F94BF10EACE4B156FA22E3D6E2CE09D96C86255F1235924AB4AFFF73FFF7BF9B7AFBB68F185A45998C457CB8BB3F3E582C49B641BC6B757CB437EF3E71F967FFBEB3FFFD3E5CBEDEEEBE2B7A6DC93A21CAD196757CBBB3CDF3F5DADB2CD1DD905D9D92EDCA44996DCE4679B64B70AB6C9EAF1F9F98FAB8B8B15A1249694D66271F9E110E7E18E943FE8CFE749BC21FBFC1044EF922D89B2FA397DB32EA92E7E097624DB071B72B57CB57E769DA4F94D1285C9D94B4A260F49B65C3C8BC28032B326D1CD7211C47192073965F5E9AF1959E76912DFAEF7F441107DBCDF135AEE268832528BF0B42B6E2ACDF9E3429A5557B121B5396479B2B32478F1A456CF4AACEEA4E465AB3EAAC05243F785D4A512AF96CF361B9265AFD3E4B05F2EC4F69E3E8FD2A22CA2E633AACD343B63483C5A80051FB550A1882AFE7BB4787E88F2434AAE6272C8D3207AB4B83E7C8EC2CDDFC9FDC7E477125FC587286239A7BCD377DC03FAE83A4DF624CDEF3F909B5A9E37DBE562C5D75B8915DB6A4C9D4ACC3771FEE4F172F10B6D3CF81C9116188C4AD6799292D72426699093ED7590E7248D0B1AA454ADD4BAD056F16FD31A45221D57CBC5BBE0EB5B12DFE67757CBEFE8407A157E25DBE641CDC0AF71484721AD93A707C2B771B9EA3A55D9D53F1DB6B7242F1ED8F73495E23FC826CFCE3A22A7BED6F5F56F21F983F2A3E8EEC7F6DD6D0DA9C70E98921B799F6E49AA539D9A049DD66FC2DB435AEAF693BE23046ABF045FC2DBB2AE8AEE72F18144E55FD95DB8AF5680B316A53C0B1D96E98AF12A4D761F92A2797DE94F1F8394FE4D05488CABAC9343BA711DBA307DFB61FC3EBD0DE2302BCB225A79A8C3DA144DCF0EDB307F9BDC663648622A6971D4966D21A1415157A1819DA92C1CC0C7181786124143C978B4D3A9F83649433B91D85A5A89BAC2A6023135ACE54976FB8814CBCB5D50E0D44226A1A65E2EAE82B16C7C2D5BF9DE069F49541A87561DC655D34AC69436158BADE22493BD38C692D80961CFFFFBF8E724DAAEE99A71C8ECC69158532B0F5FC1542EA196AD7C2552ADE46A6A68E5A90A9ACA5197B6E6BFA1A616E193600D00CCF34570BB452807192B4A86ABEDC13AFC4F4BB573F5F4CA678A1B77015BC7B6233E3C7BED324AB86A5AA998D2A642B15520995C0DCBC6D4F06B5936541FAA69E96FC78874B7B5750BD9B4D5C6DFE7C983DCD0C77047B23CD8ED9B865E5035140FAD59FE48BEE60A6EE99F2EECEA675CE7CDA4DB16C0702329EF19DC467B3517D6D6EA7D8F632081D269649FCE828EFB2CC87583683880812D659F115CEFF8FA0EDF92CC69EC1EC7D83D0D5D4F7B37C3212BECF4FA99D7DD69832FD3BAA3781AC00343F648C6A9FF91E57E086738C6A0733B9F87701D7DD5099C540ADF86CB45FD6CBF4B62956EBC4E1015C9D30C31EE0CF12A24D1768C69A26CE8639847E3B4E4C17A78136FA2C396B40AFE29A1032C88ED8F34B6BB307E1F47F77D097D20C1D6079D72B88DD3DFEC410E37DD31AF5D105BD47C9B6C7E77E89D81D61E8B1B139B15C77EB1A94DB721D61A94717C59EAB37564AF56FAEF20596AA755E63836925EDA78881BC9BE979B8653107225DACF80F465359E0671FF417CE16380ADEF68278C31924739327E41B24D1AEEAB11A950DCB953633EA602FBCB747493885CBA5B5EA66BB6B69FBA6210ABCD5B05976D917E5BD88A4A7F53E234F3686F872B45754D0E35F73C8029416C2C4CA9E69242EFAAC6FC9CDDFD1153C2DD38EB7B9A20DCB5CAF48CAA170E30B6553B3F4DDB9AAF0E31B3C9EF611FD2A1B1217DF51726691D17D487CE4F74E0DE84794F2ACF0B563641548EF73E84DE92606BDB2D6B927E0937C4A5EA4712EC94C71B3E864EDD55A2663442E5419ABF604676E774A23943DA143166CED55F7EDDD37145B62FE3AD4BF59F83D4B5EA5B5A2ECB7FDD6FE9FFAD4766981A57757592AF661CC425467C295B28520977BF77257F6C31994166F2C338648A38BAB1DF2B19EC0AC9ECB5CB01C65C5BC096B517644FE26D10E76A43B46E06282D332B1542B9964BDAB2CF005CC938574E66991D2718B36C197B1FF26E0C2BF9E40BCA8C729301C62957C89A55BA5CA8592C0B00AC15EB0CCA52F1D2DA739DB7AD945C4965650645530DE3552CE7ECD0ED695387FB714B03CCD8D5B9608B6E4A4CC6BC5456D6AC5004D5AC58CE56B38C45A364992B27B3CB1A4618AB6C196B36A9F5ADE6AF2C00305698ED2847C54B5B56AA4940DDC16D1999A1FA15CA53F3DEC7A182BB3BBA78B8707241B73C64E8BD6B3D5E57F0E640ABBF652B4DD3A8EDDB67945423EE4D4E76FDC74947EB34524E2305169B50C4C679704B9AC8D0B649DA60A43F4629374DADAC6413EE8268B9B84EE95F75DA991F968BF52628C4B6D7E69AEE1F86A3DEECF99F1FD294B653FA88D24DBCED067E7D1F6FFC5D78F257FAF8B4D50DEE4F7C15690E434A628B3E56DCDA4CA6530B764F23B5D41456315F953160BB2EE814686ACAAF14A50D1731E1D62912DB64613330FCB0454D340CAD632C0DF5C89457E9B22D66A0CFAE6C3FABB546A45B1AA5AAF669D9D5CE9D87341EE37EAA3CDC19A5A597BB2054B90EFA69E5F5F7B9EE087FC208505BA3979905FADABC2DA9D3D83B39B01DB5039B7BDE0143D7352855419F515C9D7AF51DC00595D3D83D92B17B0A631C20178A694823943FC569FC1626ABABA15BFC7B1AAEE39D0A15FA1E2318E93AC8B23F9274FB7390DD29DA7AE2C55384493AEB31F71F9B0C1718B985263F7185BA612ABE93B69D5201BBBDE66107B97D33113F6FB2575170DB2533F6E30FDE36E079C4D2DEA65378744FD1C1F61DDF51EFC8EE33499B852589A954BF05D181FE38973A952BFBEC90276DD90B75D9572921D5C1695D5EF683E1CABF2F5D16DF865957E389BAC6754AB6E4268C29E2D95A7F31AD55F6415BED3B75B5CA9FA82EFBBDA689F628B7ADF16FEA1AD4007C7E97849BAEC20FEA0ACD696F5DFA4719EE15B0D987CFB22CD984251045BC633977780E5EC6DB856D0E4E209C4E58CADF515C877B8A643A3E215459B4DAAED768ABDDD527DFEC9FA466E992468AB3E83028A27D333AE4C23897D7BF30DE84FB20B2D48B40C770252D3AB66D517C5379F850862D9565C28A227F96CC61CB886005E8147AB96200EA845B36C1AA2586C06CAB236017CC7EDCB5CBBAD28D0958401DD34116D0910933F3C62A9318C91232501ADD11900A2567629A1553A08D89565923D3815556D303C02A9F11D91638487A645BCCCA18B26D1AC76D7D89C837787E767621DA55F6AA94E3D7B5422812A7004A637320E904306D4F6548713955945DD41F796CA61CCBBE0773578F304F82E97AACBA6DD8C912D0CB74B325A0ACE39F2E4D87B93A43F95850F535D88780E95C10FA80C0292463B0440C96767E04B0624921A4B59CF7C11A13B3B076A6C32EACB2E3C7709D99D21240E2270646C0AC9819D3CCEE1C16A4BC1AA60327AF9B6304A590DA428B0D2CCF05004427EC611FA5E80FF47EF883E51E1379B066DC39180365524804DAFF78E4AF34DF58E10A8FBB90E80E7DAC8E893806883035181D9C33B10C93C2883948D6F53714A0AD0592F270060FEB363AEA068F317AE148E6644420C9FA35695CC819322998DA535E5D8FCBC1F45E802485E09B1F44FBC792C8CC884812D56B312371596F2605939CE840D7FD8AAC075EE085E74AB0A66FAF0F2C9848C3B436B248E29C0D6FB4D6912E32C96E57DEEFA01D8B603217410C671A4C57423814DB501DE332AC62D4D737BAB8A9E1D432FEED8C22FCC99C6F20166A301DC9B154725B4CF0867F5D71895D74532898E5C5CBE40CE586B1D1BA7F0300E068441B00D0B449EB62BAAA494D002E158F0E00705E1E2FD802B3F94C0B2E88A511D10529DBA4792993DAB4F82AB2D3683B9FCB51E3074F6C4A1B87C5BE377898F6C7040DA34923B034691327058998D64AD7B9688E2B2FD0C13263B91D16F74211C2CA888042546DC20194347652988939BE744040137E79811996266C848D2E973A4CC7279847CC8B06A0EC63134CD5001B230E3040BD26AD8B696F271D5865CCABB6B7B9B46F7E00C4268B934956A1B883C38761624CDC30EAB4387C6C73664F0A9826278BAE83A5CC7C5E6ED5A4B42FBD4E0E7A8147107044FC084AB080D01CAEFFF56BB8E2B31106769C91C33390BC74C8B5DB20C65ACBB241F0F428CE3960A4B7D1043EB0870EA0913106A5B99A4CB8999BB70E9ED2C3123750B28E31F00AE615313F031E16B38052A6832CA0A92342AC140D8FE1040F8DEF50512594305F57D0687A862617A93FC8D103C6C50890C2946AD2B4903C6104F454590D689D9CD6E8E2CF9961919721F852D2022A669DB720AB134388882808AF492E8B4667962E99020409095C3C29364A57A6C4BA016908C1A31F228ACDC9760D30D989752D74FE70BA2638778C105489E4D26246B3F69246E9D5B7AAA63AE03E5A8ECBCF863539F55FF391574D134D4C8A992EC44F50623AE1BD144CF957726BCAA2822B53124A808A8E9A6624BB4DA88228BB53D59125C9BEC8652C93AA0FDACCD8E232B2616C31A69019D53A471446AFDA09684895B998001AD5422C54666675DDB06073A333D5D4B9AF10E761136B53F6266E2555CCBCD292E790BC036F875181A0C815AF49772D738B93B19ED1C413261A80524F0CA36B28D904D31227FB500A66573A63FD62C9124C8406D2250CA35D204102DB90BCD00FA6623E5CDF42CD8A387F230DC091FE03A91B8EED9755DED841FED40D7C925EA1634D020058563C0500A44DCE363351251EF1AFEDAC46E2A1D0CBC9620C5D3451800994A05401C380164A0E60D9A19E356DAD6457FD8EA2DA19E158DC72186B5915A16DA20824467B18AD2351D9F22C2CAA6328B537B3BDB1BAA16062A3EB033E9C7818F50A01C4632C6EE207DB158A5405C022B75570082CA43C437D2141AFF6DDE1A2B3FA2E52BD35930A29A412CB821A6ACF0B54FA9128E118F2BAADAADB658FF1709D605189A028405CA2AB568048C4E1F64142B0964A1F70601D2882145AE7AA0B29986EE84D0B1665A5528C26480C140C0F137355151E186640D15D55E8F7A5507D1905914122EAC2C86439B90342BD0E75816363180E72FC517DF668A24F28B84C23A8105EE653874240194BBA3E681D425DE8B10252D44A22F4E8A0B7AE463B27909A664EA24DB486C4A269E493A3D17C6A4F8E3F93A973A7F2DE160AEE83E2F8128186A781533914A0E6BA2C40216956AAEFA11CFE2BE6B876F0102B502430C8CA553F6058D5680A2ABF4CAD508CE47E0ECBC07A9E3B2B8275321F70AE963FD18ECBAF0C7B01A5C0025F5CB58285BA186D0A7B2849FADA3AAE2465D006281416B6E1AA242C506350C393FFBA3BAE1E34920314058AE570550B14BD31E4C0AAAE80159A90821060B6D9300467D9D9C00399487D99ED4DF4F613A6B8F4A0473DC8BBE853EFAA03D18B7EF0554576FE561E9099CC18B893789F63B1B1660A0387708BB358B69AFD6129EBA93CF0B92CDBD4086350EFC46CAE64D4F5D94470C8F979181543EECE436F39E40F51C95A55BBE77292A10EBA8C1CB5BF90423FA84B2E438577CF34D643F359ACD681B47D77B95A6FEEC82EA81F5CAE68910DD9E787207A976C4994352FDE05FB3D35DFB2AE66FD64B1DE071BCAFEF33FAF978BAFBB28CEAE967779BE7FBA5A6525E9EC6C176ED2244B6EF2B34DB25B05DB64F5F8FCFCC7D5C5C56A57D1586D38888AEEAE6D4B799206B744785B38746F49B9397B11E4C1E7F2D0E2F976271593DC65110FAFA639C02356EEBDC6FDABA954FC5DA7DE003F13A64054A7D35754CC5DE1AB5C7E040FEE7FB936ADBFDE0451905ECB9FDD7B9E44875D8C7B4EE3B58B7FF9FAD51399C2E54A605FD4D64A52970060B10B8C3A48ED9465DB3FF8558541F7A82A0FD33BED57495912EDC3217A19A3507F539425513F32A7214766B0E4F4711B9361105B0FFBE3117169B7C7A629A17E389D87FE5577B03D3BA075ACEEDD0338A561A60AC4FE12892A8A99B755CA56DD0AB3B499C7E6B4982FB9B2B498C716B4CA28198E4CF9643E40966E623D2098A3295D3A9B205747E1B4B8E9691CEFE2C6DD9B79C3237867680E46A4FA09897A1AC78B44C86FD6E312CF04B5F558DE5554868167FF0E9D0760A78615E438EC1B5E7540635F7C61648E0D60E5C7B96594318F2D697D0CF30822563FB7A406A0977D6E4EED4DBC890E5B22F642FBD4C2B6DEEEC2F87D1C09AB11F3D89CD607126C6552DD53734A25287932F523DBFE93760DCC63075A6F93CDEFA2DAA597F3998A4067386F3697CA4BD0DCF452533959607A1AC76F81795D18FBAC86C706C0F51DE55C4621F3784C30BF20D9260DF75508084B887B311FF8E1F7EC8E33A2FB2C38DAB95DD59C744ED73D3E06C0A0F4C2944A91A4A55F23478F7D61313B038980B9A9DA2051B0B62FB8AFEA009DA2FCEA8E967A9B3613A08CA6D4C4A90A5F9462896A3E3685D37C7588255BB67966B90A6E49BC21C0FA573FB7D15E98A4E5D81635D73DB7D01A89C94D980BB4BAA71652168DD31774FA10C4645F5858DA4D1A5ECED6C672F3E274C4ACBEDC6AA4C9F88B53FD48829D78C21E48F9620CFAF11EEC45BBA5360FD2C227405C6ABBC716BBAE4DE1138150945E9AD37DF9754F8735D9BE8CB7325DE9A539DD9F8314A6C9BDB0D9DF095F7AE0777A9ACF40E074C50F94F0DB34F5C74BA63643BC5E33F224DDCD92D1AF156B174D64C1B29D3EBEC1CB3ED675D51B90985C5DCE5052D138810903132A1B493754D9C12D6952DA083202EF6DAD3CC8BCB35A31F7940361B5AC1ED9AF68CF0F6991EBB2BCA7A30B0EBCB249852C78BD8F3792DD523F9BCF10AF22133C0C6B301ED5642C23158719C0EB431ACB671BCD434BB340A6C43CB600E42E0885A3E9FA91398DD7DFE79261DB3C1B76429A766D523B913BAE4D68DCABF9D2A42031EFE3BFFE2730DFF6F9B33A72C411905038973916E1DA2718EA691C2D0CAB688EFEF82BB3C5DA030FAE360CE27C1AD105DF32F2BAA716C67490657F24E9F6E720BB13CC68EE8DCD010F97D49D3FDE51E67B1F1E857C608F89F38A941CD4CDCF5C22E3E6555E4439891D6893ED5356B811D0056270AE7E86355F5CA39F12701E9E4A36292FDBB0E063F126FBE5104557CB9B20CAA433758D2EC4F03177200269531D3CF0DABA2E7E50AAAE4353AE3AF65745CA27B8D0ACADB3421426777F20E199C30CED39A82AE6460C5FE642DD836607EB871FD5D9962D825016FB0148A467C9AB2F44A009D2ACCF4485FAD6E79FB8EE3589CF1C3B8023E50F289A3470B39A6E342A180063F5C99903BA9A9AC3E0EA9A4B06E73AEB544406C1D23597086F562842C51E023F869637567120F47830ACFDD9D3087BB384CE6086B32A8FA03D7A98CAC320484A11E8D84D1D9D419024E5499C159A54C27BB3A0A5D467A6F6B354516D3D5F5898CF48C6B4F918CF480A36579073D446379C813C56C67BF14F8AEE6FCA586C9FD0FC57AE5DAFF4E174DD7B4B09B54605A6BEC3A5543E6291F62CB27ED2FE6E53F9D46974B8FC3EA55845B69E529CAC4EE923E6D5A98A2C1794F72FE1B6C8A9B3BECFE85C7B5614385BFF237A1E85A5B74053E05D10873724CB3F26BF93F86AF9F8FCE2F172F12C0A83ACCAB254670C7A2A7EB7D22885D0C593228510D9EE566275FB444405952CDB46401AA262BCC867C4C0B736DFC45BF2F56AF95F8BFF16320BFD9D48086C7A5FF53DD3CB9558F112005DF5EDB9B0D07A394A5F130A8A72CA09F29CA471518A94822C17C56A147C2E924DD52BD24A49BE3A94AF1A88BF04E9E62E283E321B7C7D4BE2DBFCEE6AF9DD394BB3FCBC2847923DF7562A96FD8CC0C3D76B7B5F27AA1617BDA8F874F1E6DF3F8997629F6A628F16E5EDDBD3C5C5236A76FC1A87FF38D00A1F699750B5B17DF658D367A63070E1B57863CEA80E5D32A7F50524D375963D235F3932B47A4B7C2E4B6CD3B340758D80C6E30FB9C699E15834E88437DB4E655A154118906E4A6C412010C0D8E931611AE5123ADA09529183C8AE2B5042BD10C2A431EAB138CA74993BF58A6EB197CDC362F6B5E4B0F24DE599FB975DF0F55F7DC14F480834C389E2B4689F16ED07BE68B389904E23F034021D979DD300EC6B8461199F8E76A8F9EED15EE656EF6388DE90774444ED9BF40D4CCDDE678036FF93EB34D0E57CEA3113C8823279A97CCCECB298FEA77736FB95379E79CD0EC474EF85A94BAF55D1F81CDAD360F26AB913E9326AB9D3A8D369799D0799D45AEE5A96B269B9CBD8F914DACD1F4DBD214E5A809C57DFC08C7E32B68761F4646CF735AD4EA30F145D89D30B6B9C32F9C83CF130FCA0F67D32C5A5B452ACBB17E7E7BE2E421B7F8A6F00E3D75D02B3C950AEDD5A4E8A1A80369B144D45DB614B0C2446B35B59640ABDB6FDC21D83CBED8F44C1DC3E44F96933B039F152D7EEC38790B4CD8E0DAE721F2E9A6C6F3D0D993ACB9B3B1536BB9B3B952EAF5B0F79D8746EEE64DA9C6B761D5B57EBD3A562D2373B06F8DA7DF8A812C629F7B9D6B35B973E0EEC1923F574E9DDF0DB59134252B6B87EE4A42471FDC871F9E1FA9192D2C259429AAFDE6BB61232C9D931C2D71EEE8CA1F5E2F876AC3FF795B4AF797174CE164CD4C6091FDF003E009D0019E22AB2290922373BB2958E6CC25D41E49AEE30C2ACF4C1BEF881F62BB568E8EBC7D652D779E306A28EE68FEBB76235F9E3DCCD37294CD5720FC557EFB55FE9421D2D075053B157EB8ECE93F63E93C06D0B175B65D73E5B778895BE8E0C7EF813789BF4CFEFE90A9301D02FE13A25A05FA24D8AC059B889DA1A1C4C20E9C387EBE9BEEB74DF35AFFB2E26B9E069FC9DC6DFC9BD7AE401582659FC06469EBF8D6E97C8D1ABBDC3677454907E627F2E2CA476B4433657D9E2B0C1DAC31F0AB237C3DBB52A0C5D2E6E90E84B8FD7C98F6E4029EC3801480CDFC11723F4B0361FC511F42F2083E5265F243048DF2A73641876AE3AC7850D1A0CFAB5C7DDBFEF5BFFB1C085F526938643D40E6F9320DF002A528C2C3E24115AB191AEC8828124DFA56BDB21CAC37D146E288757CB0B49776823DD4D12DE4A57866FE64F5233145EA438650D8322A421CBD32094531C5FA761BC09F741A4165BA8666824151DD53620BEA9E6EA224FB65A17264D2B228F658EDA868581A6D31797E5C509759D1BC7C4B86313773084D9C70F0C5D8C6826ADCD1B47B56B5438358C044F2D9EBAF8EE81014A94EF01A08A4F1AAFE974264D26DFE3F50BBEBBCFCFCE2CD64177B4FA82152BCAF86B9F0D8C8DB2E58F092A3917BCB6D799405CB0CBD9F76EB8E2023BD1269A02C34E5558D8F1C0D07A8BE7DD07DB5525C89F6E92EA9437F5DAD71FBCC7BF063A427ADEEBA0E9AC351EBA6632714D8C2F8BB96BDE0063BD856662C2CB1F66E08D3AFEFD43831920E3F1A3ACB407E7812EBBBDC243C193F9EE616E40FAA45019042118353D0E42DDD1086E08FAE167E61BC0E9E0529FDC1B9CA537E1B1F20CE0309BC0A7E4E2BB21CF05469C44EC8EC195D9CB47840476DADAE7085AB7D537C7982730581E29FBC183515342ACE9A450800F48FD1C22CF0E122E87C223E242606F7A70987D946FB09544416D149C8C698CDAC3648EF080BEEE7182C7B8F030FCACE47838C13E1039E09987D9CAC3C67ECAF4D9B743C2C8FA5CC30FA4F0CF95818D1A7C2B731244D5A15F684F335F9B6CBAB77E748C8801849D1348AE559FBF9C061EE3DF95CF032963DF90BBE165E23B71092E5D709EA67B856F49F2BDCBBC3C5EE820AA98137C749FBA1CD114669299E8A0E3DABD73DD4D5B76DA88FB69313FCDA40861F3EE9C2032178848C994A6C50809B66399B263A2606CB3D5ACEB9B8C6C9376B990DE7194CBBCF10130EA5D9D0506A0F49C93C2C1E43BCE431FA85D284ED42E1EDA919AF491ED791FB99A7C46FB848FD1F161F43DF21161C2A40D3DD91323AD2562A2D76901502438D1F458950445EEB5EAF9DC31C0F03A332030AC4D0F846A6B3396EFCF3C8EAEE67B6235FD02D19ABAC626E6005E8420E8A4970F602BA24C5680616406BB11192523199A7340C8889388133E663387080EEDDDB2370FB7761BFBE6A138B51BDB4373F369E7F9696F76660225B7EBBE87022ABBABC09920AB489BC6E6124371C496617B977B6E8E982AC11D43A77A30083250F18681432989493B42FEB711FAFF65996F88D6C9690D92B650DC92F212F04590079F214F90A2D69AE432DFCBC5CB3685110488F5E68EEC82ABE5F67361185799909802198010BE2DD6935E6A8A7D09B5C466AAD135844D8252A358418801B8AC35335D3C888E9BAEA4013B5DC88B969F6A86EB3CD8653EC41260FB5CA1D0A457380F23ACD5FAB5A2C93A8CD058EF6CB83BAE73B69452DF6CBCBF1D0F4D4CB48689A6989E8BAAA4B1E679F751AC03F8528A7EE00B9A6A4225BE4666634171E1940219D3578D61B184A23DEB31CB9E26612DB365146D77C50C5AAFCFC8E526EB17603B24D94772963E4432C6A0C404638A28E4624A99B55C6D8DB046ABB78AF6AABD9FAEADCA20921AA91E43D48B37E633AA0EF54AF04B210D96CD5E68DBBD801B162EF6CCA71085BC5211E5848236CD985C3AAB8209365D30D594060616A06AB4D96BD5821791AC4F94AA38934164BB32829656BC9ADC55C8D976C64A4473178EA046D9942D29B18F8752166B7119EB0ACBCF3706E2607BB32227BE1B4C697C26390BC52952D0416272D62D2B63FDC27C68BAABBF8FDAE45C692A5D6932ABC1E2C9F6382F1BFBDE445D806D0D106C0A0C85306E5B600C2F3479D8984B817B8778569FB5E6A656DAE4A813123799AB4F95F169BC4501DA91B293A6728BE94D89CD8182B1F2A04446E329CD6E89E9A32621EB8E4A41AA043DD8E53AA48DF9AE90F51DA4DADC970A697B13EA481314C096BBF8CE9BD8AC658BCB8D2720E9633BBBAAAD87B89D458A0B8BE5D4F063FB4E20349AFB42A104B37C193E81AFA83E864A6AAF102B95409E24C7AA122CB501AE10A36408BEAD03889E7C7CCA5263DF0EA0ACE6ACD4444DB0B72FE0C8CC4A513F9ABB1AD0FD375274D01DF70C14C21C589B2805754C507A70C852A1E7E4F35010175DAC986BD120649F624CB01473A1B30AF9F110DB63574011C7A1125C8AF3F037410E2C9A18FFA89052192AE96F5B358AD8489C9F427A93C8C001CCA80B851D25BB41F95388DEB23409853B6285B0215C0A2DA0915E47330354B7CA0A09019756CCD95766B47A3EB9904D188E424E3052C72780C7345CD03013E5B199C5E4E6E3E00CD4A7F4720865286637B3D00B3FB8984A118AC0018B7367B6DA44A7CF36B38E3F85B1DEF1E6FA427DEAC75397DB26CC417192F337A026B58338C73EE0175C72CE3D5788CF7A3C9515AB07C66236DF536DFD9BDB7797ABCAA7A77E407FE6491ADC9277C9964459F9F472F5E110175F1EAF7EBD205978DB91B8A43463AAFD90F1A76ECBBC896F8AE15F7A770B1C35459AD7B5E2DF913CA0EB48F02CCDC39BA0348C0A1551BB7DB9F82D880EB4C8CBDD67B27D13BF3FE4FB434E4526BBCF1177E05BB887ABDABF5C493C5FBEDF17BF321F225036C362297C1FFF7408A36DCBF72BE03BB70889C2EFBCFE967CD19779F14DF9DBFB96D22F496C48A8565FEB2EFF91ECF685D59BBD8FD7C117E2C21B85DE5B721B6CEEE9F32FE1B6C0214644DF11BCDA2F5F84C16D1AECB29A46579FFEA418DEEEBEFEF5FF0141DCB93FD0C70100 , N'6.4.4')

