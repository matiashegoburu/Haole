<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneradorDeFlyersHtml.aspx.cs" Inherits="Haole.GeneradorDeFlyersHtml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 167px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 100%">
                <tr>
                    <td class="auto-style1">Nombre del Evento:</td>
                    <td>
                        <asp:TextBox ID="txtNombre" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Descripción:</td>
                    <td>
                        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Imagen:</td>
                    <td>
                        <asp:FileUpload ID="fupImagen" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnGenerar" runat="server" Text="Generar mi flyer" OnClick="btnGenerar_Click" />
                    </td>
                </tr>
            </table>

            <asp:Image ID="imgFlyer" runat="server" />
        </div>
    </form>
</body>
</html>
