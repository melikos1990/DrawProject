CREATE TABLE [dbo].[VENDOR_HEADERQUARTER_NODE_RELATIONSHIP] (
    [VENDOR_NODE_ID]                  INT     NOT NULL,
    [HEADERQUARTER_NODE_ID]           INT     NOT NULL,
    [VENDOR_ORGANIZATION_TYPE]        TINYINT NOT NULL,
    [HEADERQUARTER_ORGANIZATION_TYPE] TINYINT NOT NULL,
    CONSTRAINT [PK_VENDOR_HEADERQUARTER_NODE_RELATIONSHIP] PRIMARY KEY CLUSTERED ([VENDOR_NODE_ID] ASC, [HEADERQUARTER_NODE_ID] ASC, [VENDOR_ORGANIZATION_TYPE] ASC, [HEADERQUARTER_ORGANIZATION_TYPE] ASC),
    CONSTRAINT [FK_VENDOR_HEADERQUARTER_NODE_RELATIONSHIP_HEADQUARTERS_NODE] FOREIGN KEY ([HEADERQUARTER_NODE_ID], [HEADERQUARTER_ORGANIZATION_TYPE]) REFERENCES [dbo].[HEADQUARTERS_NODE] ([NODE_ID], [ORGANIZATION_TYPE]),
    CONSTRAINT [FK_VENDOR_HEADERQUARTER_NODE_RELATIONSHIP_VENDOR_NODE] FOREIGN KEY ([VENDOR_NODE_ID], [VENDOR_ORGANIZATION_TYPE]) REFERENCES [dbo].[VENDOR_NODE] ([NODE_ID], [ORGANIZATION_TYPE])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商組織服務企業別主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VENDOR_HEADERQUARTER_NODE_RELATIONSHIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商組織結點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VENDOR_HEADERQUARTER_NODE_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'VENDOR_NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總部組織結點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VENDOR_HEADERQUARTER_NODE_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'HEADERQUARTER_NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VENDOR_HEADERQUARTER_NODE_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'VENDOR_ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總部組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VENDOR_HEADERQUARTER_NODE_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'HEADERQUARTER_ORGANIZATION_TYPE';

