<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneradorDeFlyers.aspx.cs" Inherits="Haole.GeneradorDeFlyers" %>

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
                    <td>Tamaño de Fuente: <asp:TextBox ID="txtNombreTamanoFuente" runat="server" Text="60" TextMode="Number"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="auto-style1">Descripción:</td>
                    <td>
                        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>
                    </td>
                    <td>
                        <p>Tamaño de Fuente: <asp:TextBox ID="txtDescriptionTamanoFuente" runat="server" Text="20" TextMode="Number"></asp:TextBox></p>
                        <p>Margen Superior: <asp:TextBox ID="txtDescritionMargenSuperior" runat="server" Text="0" TextMode="Number"></asp:TextBox></p>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Imagen:</td>
                    <td colspan="2">
                        <asp:FileUpload ID="fupImagen" runat="server" />
                    </td>
                </tr>
                <tr>                    
                    <td colspan="3">
                        Ancho del flyer: <asp:TextBox ID="txtFlyerAncho" Text="800" runat="server" TextMode="Number"></asp:TextBox><br />
                        <asp:Button ID="btnGenerar" runat="server" Text="Generar el flyer" OnClick="btnGenerar_Click" />
                    </td>
                </tr>
            </table>

            <asp:Image ID="imgFlyer" runat="server" />
        </div>
    </form>
</body>
</html>
