namespace tree_lang;

public partial class TreeLangParser
{
    public static AST ParseFile(FileInfo fi)
    {
        if (!fi.Exists)
            throw new FileNotFoundException("TreeLangParser error: file for parsing not found", fi.FullName);

        var file  = new AST.File(fi);
        var lines = File.ReadAllLines(fi.FullName, System.Text.Encoding.UTF8);

        return ParseFile(new List<string>(lines), file);
    }

    public static AST ParseFile(List<string> strings, AST.File file)
    {
        return new AST();
    }
}
