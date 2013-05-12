using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Security
{
  public static class XmlVerifier
  {
    private static readonly byte[] MODULUS;
    private static readonly byte[] EXPONENT;
    static XmlVerifier()
    {
      MODULUS = Convert.FromBase64String("zVsiPhUCT4jFmpmNwHcJNKQwwtDyMK5UgfxehQUgb7YAKUVl0XTeaCaI96uEGFa+B+iN2bZViLUuX06kXtE2VfkqPTEg46/RcIatBwYL+WS1b7pAXhzFZCJ3CzqWHjixHsTzk4wtzITUggGd+LRDL0lMoBV2X/MTo9OlWYRrta8=");
      EXPONENT = Convert.FromBase64String("AQAB");
    }

    public static void Sign(string keyXml, string filename)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(keyXml));
      Contract.Requires(!string.IsNullOrWhiteSpace(filename));

      using (var rsaKey = new RSACryptoServiceProvider())
      {
        rsaKey.FromXmlString(keyXml);

        var xmlDoc = new XmlDocument();
        xmlDoc.PreserveWhitespace = true;
        xmlDoc.Load(filename);
        var signedXml = new SignedXml(xmlDoc);
        signedXml.SigningKey = rsaKey;

        var reference = new Reference();
        reference.Uri = "";
        var env = new XmlDsigEnvelopedSignatureTransform();
        reference.AddTransform(env);
        signedXml.AddReference(reference);

        signedXml.ComputeSignature();
        XmlElement xmlSignature = signedXml.GetXml();

        xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlSignature, true));
        xmlDoc.Save(filename);
      }
    }
    public static bool Check(byte[] modulus, byte[] exponent, XmlDocument xml)
    {
      Contract.Requires(modulus != null);
      Contract.Requires(exponent != null);
      Contract.Requires(xml != null);

      var signedXml = new SignedXml(xml);
      XmlNodeList nodeList = xml.GetElementsByTagName("Signature");
      if (nodeList.Count == 0)
        return false;

      signedXml.LoadXml((XmlElement)nodeList[0]);

      using (var rsaKey = new RSACryptoServiceProvider())
      {
        rsaKey.ImportParameters(
            new RSAParameters { Modulus = modulus, Exponent = exponent });
        return signedXml.CheckSignature(rsaKey);
      }
    }
    public static bool Check(byte[] modulus, byte[] exponent, Stream xmlStream)
    {
      Contract.Requires(xmlStream != null);

      var xmlDoc = new XmlDocument { PreserveWhitespace = true };
      xmlDoc.Load(xmlStream);

      return Check(modulus, exponent, xmlDoc);
    }
    public static bool Check(byte[] modulus, byte[] exponent, string filename)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(filename));

      var xmlDoc = new XmlDocument { PreserveWhitespace = true };
      xmlDoc.Load(filename);

      return Check(modulus, exponent, xmlDoc);
    }
    public static bool CheckUsingBuiltinKey(XmlDocument xml)
    {
      return Check(MODULUS, EXPONENT, xml);
    }
    public static bool CheckUsingBuiltinKey(Stream xmlStream)
    {
      return Check(MODULUS, EXPONENT, xmlStream);
    }
    public static bool CheckUsingBuiltinKey(string filename)
    {
      return Check(MODULUS, EXPONENT, filename);
    }
  }
}
