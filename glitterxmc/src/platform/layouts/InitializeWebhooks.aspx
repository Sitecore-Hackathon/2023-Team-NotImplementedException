<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" %>

<script runat="server">
    void Page_Load(object sender, System.EventArgs e)
    {
        new Sitecore.Pipelines.Loader.InitializeWebhooks().Process(null);

        Response.ContentType = "text/plain";
        Response.Write("ok\n");
    }
</script>
