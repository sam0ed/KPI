library(xml2)
x <- read_xml("<foo> <bar> text <baz/> </bar> </foo>")
print(x)