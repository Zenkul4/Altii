import { useEffect } from "react";

interface SEOProps {
  title:       string;
  description: string;
  image?:      string;
}

export const useSEO = ({ title, description, image }: SEOProps) => {
  useEffect(() => {
    document.title = `${title} | ALTI Hotel`;

    const setMeta = (name: string, content: string, isProperty = false) => {
      const attr = isProperty ? "property" : "name";
      let el = document.querySelector(`meta[${attr}="${name}"]`) as HTMLMetaElement;
      if (!el) {
        el = document.createElement("meta");
        el.setAttribute(attr, name);
        document.head.appendChild(el);
      }
      el.setAttribute("content", content);
    };

    const defaultImage = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1200&q=80";

    setMeta("description",         description);
    setMeta("og:title",            `${title} | ALTI Hotel`, true);
    setMeta("og:description",      description,             true);
    setMeta("og:image",            image ?? defaultImage,   true);
    setMeta("og:type",             "website",               true);
    setMeta("twitter:card",        "summary_large_image");
    setMeta("twitter:title",       `${title} | ALTI Hotel`);
    setMeta("twitter:description", description);
    setMeta("twitter:image",       image ?? defaultImage);
  }, [title, description, image]);
};