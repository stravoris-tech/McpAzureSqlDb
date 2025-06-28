namespace McpServer.Models;

public class ProductCategoryModel
{
    public int ParentProductCategoryID { get; set; }
    public int ProductCategoryID { get; set; }
    public string ProductCategory { get; set; }
}
