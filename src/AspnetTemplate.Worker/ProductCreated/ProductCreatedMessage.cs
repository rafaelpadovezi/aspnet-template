using Ziggurat;

namespace AspnetTemplate.Worker.ProductCreated;

public record ProductCreatedMessage(string Code, List<LinkMessage> Phothos) : IMessage
{
    public string MessageId { get; set; }
    public string MessageGroup { get; set; }
}

public record LinkMessage(string Value);
