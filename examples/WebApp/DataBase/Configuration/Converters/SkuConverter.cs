using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Catalog.Values;

namespace WebApp.DataBase.Configuration.Converters;

public class SkuConverter() : ValueConverter<Sku, string>(sku => sku.Value, value => Sku.ParseOrThrow(value));
