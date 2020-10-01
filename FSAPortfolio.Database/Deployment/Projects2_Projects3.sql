﻿ALTER TABLE [dbo].[Projects] ADD [FirstUpdate_Id] [int]
ALTER TABLE [dbo].[Projects] ADD [FirstUpdate_Id1] [int]
CREATE INDEX [IX_FirstUpdate_Id1] ON [dbo].[Projects]([FirstUpdate_Id1])
ALTER TABLE [dbo].[Projects] ADD CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_FirstUpdate_Id1] FOREIGN KEY ([FirstUpdate_Id1]) REFERENCES [dbo].[ProjectUpdateItems] ([Id])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202010011629278_Projects3', N'FSAPortfolio.Entites.Migrations.Configuration',  0x1F8B0800000000000400ED1DDB6EE4BAEDBD40FFC1F0535BE46472418B36C89C833497DDA09B4D90C91EF42D50C6CAC4AD2F535B932628FA657DE827F5172AF9AABB25D99EF8648305761359222992A2448AE2FEEF3FFF3DFEE9258EBC6798E5619ACCFDFDDD3DDF83C9320DC26435F737E8F1873FFA3FFDF8EB5F1D9F07F18BF773DDEF90F4C323937CEE3F21B43E9ACDF2E5138C41BE1B87CB2CCDD347B4BB4CE31908D2D9C1DEDE9F66FBFB338841F81896E71DDF6E1214C6B0F805FF7A9A264BB8461B105DA5018CF2AA1D7F591450BDAF2086F91A2CE1DCBF589CDCA4197A4CA330DD3DC76010CC7DEF240A01A66501A347DF034992228030A547DF72B840599AAC166BDC00A2BBD735C4FD1E4194C36A06476D77D3C9EC1D90C9CCDA8135A8E52647696C0970FFB0E2CE8C1FEEC463BFE11EE65FC1A05732EB828773FF64B98479FE294B376BDFE3F11D9D4619E92BE7F22E666696EF5210763C59BF9D464FB03A913F3BDEE926429B0CCE13B841198876BC9BCD43142EFF025FEFD2BFC3649E6CA288A61B538EBF310DB8E9264BD73043AFB7F0B19ACD65E07B3376DC8C1FD80CA3C69493BC4CD0E181EF7DC5C8C143041BB5A018B24069063FC1046600C1E0062004B384C080056305EC1C2EF2778D0DEB215E54BE77055EBEC064859EE6FEEFF12ABA085F60503754047C4B42BC04F118946D208BE378D68A542BE81B2C03A2414E322E077F88B743BC8B4D96E8257CE0206211CF4598E5682B98CE631046A363F9F40704413CECC210B1DCE11D2647205ED788CEB09049A34427DC965896FE0D2ED175F2398D820556A94D6EBDDE2A18CD0F34B08FF5D7CFBCE21F47B4AFA5BC6E9E0081D853EA05940F714F5FDCB7279F065AE80DA40FB14F5FECBDA5FD21E40E21577C6A514A24BD3FC4A9A0439B8639E09CC17C9985EBD287D3CC676F08645F2008EE055975B13B4CB3422C7A01779C7F11C810395589272CFDC0932571F69D879FBFACB1AEC0E03C095C867F0699EBD02FB85F8EBEAD03FCAF2DCF8B53BCE1D0AFE0395C150B4B0DC4F76E6154F4C99FC27519FCA8EDCD3DD3EF224BE3DB346A9719FDF97E916EB225E144AAEE7307B21544E664D27CD2D2C97614096518AEA294E9644D2A5E3E7A128B0E12D2C8BA5392443EDA9252CE20D752D3F41109AA3E2969AABFCBC8B2DD114B589708C6BDF7C616D4C72E69B64B8AE6C3D270BBBAC61248F0050D7C3893EC33AFC9D27ACACA55C6FAEBEAA5D6EAE53D3B4458798A9EAA85A8EA6E6B2EEA109FD114EACE3AE2CB3E0664571DAD092E9D65337ACBBE5A724917136A8B7ED6C4D647FF7EC6B846AB32C635F9A664511EA8111FA9FE3A5E36DD0CF8D9F6EDB5939050B3637C9AFCFDB1596CCDC813760F7F9521D9DE409EFF33CD82CF207FD2E03A1CC46DA2EEB186DB5B98EB35C9FA249CBC673AB58B92FF26AC44A183CDF23BC9F3741916F4A84E72EC36C7CE18FB4B9EDD9E574A4F6A94B034F19A0CD77815624AE7FEEF04061BA36BCCAB808EDDB359847BBBBBFB3CDB2806D9F2ADDA5BCDA7C06FB4A3F18ADBA86944D579605CC6149BB805B9EC8E3E1E5B98138188A73A7C8CCA9B766336A75BB24B8FC6237197177151878BE179C5C4303AE8960734047A1D38220D83D8707D10563061922E8AE531934198218DB4F4E3064907828F3023871A109DE2ED129FD2C2048907B03059866B101990C48D353CBE11C93458F82F67700D1372F23260B6097A219228D2D2A0E40E995DEC72D12F12E3EA143E13F01A469FE83099C3EED45B7928FCDB541A8A9346CA5287F8DF54496A6FB74BA8421CB2535544A9AAA1BA581D51157B690D37C12D2A0EC70413CC74CCF04DD447F0795492563B40ADA8CBA081B9A151FA4C144CC61FEB524C27C55151B105CD5131D50435E7226F417B4ADF158F417804CC6A15AEA33CE443117016A246789A55E028AFDC7F5E2308E00544E2D4F0E1B57599652A2128170BEA06A6EB084A80D49B58D778D15F85329AA47EAD19ECC29FD1C0ACFC1D3360CDA15F0791F20CCCA06A60998268EDBF0618BD4974802D628D1248A50DE206530AAD218CBB49A00699DD3DF02BD03A2CD3CC51CE3561895B07624404BC62F3315396734E5CAD579A093F65C11ABB70CD903CE4023434E8CAAC8CC1AE72B91B714B8CE058C57006E51513B51121D7566E048E51E6CC846B8AD88E75746748EE89F11C113A63DA8762239B8BA2649F3ADC631CF0112764C4246988C78AF53D98C326C0A8B9A38E0099C7805CF9238DFA6C8D4145FA8D863142E8A23B78E1CC083A5C31A2AD6E2F96D5F3967AE346FEB8EBEC790F7C6C0D102FCB446EE89D4B33F7929A4775E4D33043E950525058E7C2980FF5D55DE3FE34DF8E67E543C8AAE178A67831797C05D6EB3059512F28AB166F513E9F3CFD6161FFAA302E61CC968CB6F1CE5A8309A5195841EE2B468D292D2CED1940E0A138819C06B1D04D70F61487F41A9DC49F13A5579FE0EB41E4E72AA1529665A051A896A51778963171B48B7B7AB9F8C5D11E79D40A22904932034ED36813276AB75F3DBABC9DA7C7972D2284E31947BEE0D40BDCE2F4979780917C2A0BD95B32F2D3BB81505403C79147F3329006D1349AC3A15EFED190A8667358D5DB3E1A4ED5640EA37EB94703A9DBCCA15099293420AA793A7A2BF561FBABB12496E3A0D32650BE5783C3F88443094CEA0E9B4B4A31FCFB1611ED740E2527A5036E2E2B0D88EF5B5EC349C95D38DB9249858E0742358F215F1504E659190D88F9600EAFB956A66129EF9A755CAA5F93B14CAA5B2DCE4DED9330E6E4D4369BC3121E99D110858F16E728FEF51973A2E23F9AC3659EA5D130990F16D2E5334C182977A59FA8E1F2EFD88483A916EADBDA2E3A40319415A36E949CED990EC6A8964D90A02E5D400DCDE544AF8455DCF032608A160B2B52BD17624C48D536198D2C434DBD95B0B88CB4D73BF9B071546D48E5689F22D0A0DA568B05C03C36609600F3C566C361D225D8ED469B4931BE12B241C70EDBC8DD52DB9A416EB8B5C923D156B9B5EAB87C16B96AA4CD0C2879A20B45516F62950938CE4B4F4BDF699A0461918E759993772DCD9B160B16F041EB0154ACBEB2B756AE7AE0386AC5DEC73BCAA802328A2AB1B908935222E5B4C7509F3285C15E7BCA7123290F9DA0E0AA3B058C715487CECC9896E628263D82E250991CD6CA438D1D4781841C0D4729B57046512421516552CAA49BFC500AC5E6B49829123BC630B0A596872C6BC5510EACD72EE43CBB6A8C2C4767529AD231EF6E65116EECF92ECDB1BE6A697E6F6EECABDBF2EEC2C7C2F579D9C5F7300B9EC3805C9D2F5E73BC3C774987DDC53FA2D328C48E44DBE10A24E12376B9CA67EFFEC1DEFE015739793A558C67791E30AFF26D4B195F26017C99FBFFF2FEBDFDB7FD21E17AE7EB7DCB47F8A57F5B22489E41B67C0299F004BF853946E9E077C753AE80AF94AD075D7C15C10AF57A8702CC94E71D0A285B8DD749B544A042F1096264D1C8C577DF8752CA16FA6F62F0F2DBA196B7F22EFA83657A96A99EEC7FB04DCBB6EF6127A16E84357674DFDA8E76EEFA0E065F529D5441ED9E356CAE1869C16BB5F4C99823EFF2AFF7D5B01DEF3AC307D5236F0F2B862566BEA46981BA6F4153D9AE65565C485AE0D4159CA2E0A92B3849015457508A82A8C6726787F791BFBCBC2AA705F680F66DA7C40D379FD2A02538DFAB8175D0B176A44A166E35330D4FB4124854CD4C8B6D5762AE98BA982E064FB8F7B1632D37BC97F96E2F0F2CC55B0FEC85BD09405B22AFC6F5C1CDC62BEDF0D36347B0348A128DEFCEB80CB7B6F9528903B9D3B2CA8852D087F64736692144530564065BD8D76D9529143319FA54099461A0B7E1B7AB49D3F192C064157A0E0569B4B96F2252834C8BEDD533B228E048E52AD817A89A9CC6A89E504D4449B4D9136FA31E26652CA98C04A7629253D414FDA397A9E88B2663E24DD4C5B8B227978BE05C5B73BAAAA37B873311F5E9CA95D862813D8B82A7AEE235531EB7F28E7D14C55268C3688A112A83E4886D16EA549601F9D091B7D3915F5231D761CFB2DBD4826D9F5BDF5F69D6816AB14EE36C31DD23051DAB7E13B5302EB9CAE450B99644658BE338946B75D2027D8D96C13541F1CE4AC43395A2A962B91D5E6ACA8AA85D0551CBCCC3B91F3CA458DA658C4F5BF388C755EF38029AE671870483A2C896005B16D81311C97A49B1CA3A9A915039FE2ADCF553043552454143053ACA6154A1A413D8D568AD6BB7EA4AB76AF018C3A777141526BA8F06A7AE42198FBD345E02C2B25986435A3D6C2A9561650B42594546345BDA4380F621444756B513B36E062DF8CA5823BA5AD4E4D930782157C66689A56BA6CE90F1EAB40AD6555E2B66920C1AA502ABEB34E47E89E425CC70D31FA7C6EA2F89013D6BA8F63190234F6DC84AA926A4BEF18A1EB826AAE4D02F94B0D431803E22B57525CCA76951F2547C28855DA64D42B234CADFCE601EAE5A10E4095882D94F3B4B4D9FCBE431AD5D378EA2BACB0D9B25710511C0720427190A1F01DE0FB394B0A8F8BF297F06D10692D7190F30B84CAE3768BD4178CA307E8898EA47C4F7D3E12FEABAB2341F5F1779BAF91053C064864415AF933F6FC22868E8BE90A467284010A7B2CABB21B24424FF66F5DA40FA9A2686802AF635BEF01D8CD71159A5D7C9023C4317DAB0EA7D812BB07CBDA9DEBBA981740B8265FBF15908561988F30A463B1EFF8A7538885F7EFC3FA645FFF04E910000 , N'6.4.4')

