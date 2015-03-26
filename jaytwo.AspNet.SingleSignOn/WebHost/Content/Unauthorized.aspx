<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unauthorized.aspx.cs" Inherits="jaytwo.AspNet.SingleSignOn.WebHost.Content.Unauthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unauthorized</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Unauthorized</h1>

            <p>
                Resource: <%=Request.QueryString["ReturnUrl"] %>
                <br />
                User: <%=Page.User.Identity.Name %>
                <br />
            </p>

            <p>
                Authenticated, but not authorized.
                <br />
                <em>(It's not that we don't know who you are, it's that you don't have access to this page.)</em>
            </p>

            <p>
                <a href="<%=GetSignOutUrl() %>">Click here to sign out.</a>
            </p>
        </div>
    </form>
</body>
</html>
