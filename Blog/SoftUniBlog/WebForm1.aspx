<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SoftUniBlog.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Име:
        <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
        <br />
        Картинка:
        <asp:TextBox runat="server" ID="txtImgPath"></asp:TextBox>
        <br />
        Категория:
        <asp:DropDownList runat="server" ID="DDLCategory">
            <asp:listitem Text="Professional" Value="0"></asp:listitem>
            <asp:listitem Text="Semi-Professional" Value="1"></asp:listitem>
            <asp:listitem Text="Home" Value="2"></asp:listitem>
        </asp:DropDownList>
        <br />
        Тип:
        <asp:DropDownList runat="server" ID="DDLType">
            <asp:listitem Text="Bench" Value="1"></asp:listitem>
            <asp:listitem Text="Cardio" Value="2"></asp:listitem>
            <asp:listitem Text="Dumbbells" Value="3"></asp:listitem>
            <asp:listitem Text="Stands" Value="4"></asp:listitem>
            <asp:listitem Text="Fitness Machines" Value="5"></asp:listitem>
        </asp:DropDownList>
        <hr />
        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" />
        <asp:SqlDataSource ID="Equipment_DS" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT * FROM [Equipments]" InsertCommand="INSERT INTO Equipments(Name, ImagePath, Category, Type_Id) VALUES (@Name, @ImagePath, @Category, @Type_Id)">
            <InsertParameters>
                <asp:ControlParameter ControlID="txtName" Name="Name" PropertyName="Text" />
                <asp:ControlParameter ControlID="txtImgPath" Name="ImagePath" PropertyName="Text" />
                <asp:ControlParameter ControlID="DDLCategory" Name="Category" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="DDLType" Name="Type_Id" PropertyName="SelectedValue" />
            </InsertParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
