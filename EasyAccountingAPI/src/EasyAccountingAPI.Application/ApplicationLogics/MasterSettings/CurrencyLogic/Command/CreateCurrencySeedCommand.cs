namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Command
{
    public sealed class CreateCurrencySeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateCurrencySeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly ICurrencyRepository _currencyRepository;

            public Handler(IUnitOfWorkRepository unitOfWork, ICurrencyRepository currencyRepository)
            {
                _unitOfWork = unitOfWork;
                _currencyRepository = currencyRepository;
            }

            public async Task<bool> Handle(CreateCurrencySeedCommand request, CancellationToken ct)
            {
                // Prevent duplicate seeding
                if (await _currencyRepository.AnyAsync(ct))
                    return true;

                // Seed default currencies
                var currencies = GetDefaultCurrencies();

                // Transactional operation
                await _unitOfWork.BeginTransactionAsync(ct);

                try
                {
                    await _currencyRepository.BulkCreateAsync(currencies, ct);
                    await _unitOfWork.SaveChangesAsync(ct);
                    await _unitOfWork.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    throw;
                }
            }

            // Master currency list (finance-safe)
            private static List<Currency> GetDefaultCurrencies()
            {
                return new List<Currency>
                {
                    // 🇧🇩 Base Currency
                    new() { Name = "Bangladeshi Taka", Symble = "৳", BaseRate = 1.00 },

                    // 🇺🇸 Americas
                    new() { Name = "US Dollar", Symble = "$", BaseRate = 110.00 },
                    new() { Name = "Canadian Dollar", Symble = "$", BaseRate = 82.00 },
                    new() { Name = "Mexican Peso", Symble = "$", BaseRate = 6.50 },
                    new() { Name = "Brazilian Real", Symble = "R$", BaseRate = 22.00 },

                    // 🇪🇺 Europe
                    new() { Name = "Euro", Symble = "€", BaseRate = 120.00 },
                    new() { Name = "British Pound", Symble = "£", BaseRate = 140.00 },
                    new() { Name = "Swiss Franc", Symble = "CHF", BaseRate = 125.00 },
                    new() { Name = "Swedish Krona", Symble = "kr", BaseRate = 10.80 },
                    new() { Name = "Norwegian Krone", Symble = "kr", BaseRate = 10.50 },

                    // 🇮🇳 Asia
                    new() { Name = "Indian Rupee", Symble = "₹", BaseRate = 1.30 },
                    new() { Name = "Pakistani Rupee", Symble = "₨", BaseRate = 0.39 },
                    new() { Name = "Sri Lankan Rupee", Symble = "Rs", BaseRate = 0.35 },
                    new() { Name = "Nepalese Rupee", Symble = "Rs", BaseRate = 0.81 },
                    new() { Name = "Chinese Yuan", Symble = "¥", BaseRate = 15.00 },
                    new() { Name = "Japanese Yen", Symble = "¥", BaseRate = 0.75 },
                    new() { Name = "South Korean Won", Symble = "₩", BaseRate = 0.083 },
                    new() { Name = "Singapore Dollar", Symble = "$", BaseRate = 81.00 },
                    new() { Name = "Malaysian Ringgit", Symble = "RM", BaseRate = 24.00 },
                    new() { Name = "Thai Baht", Symble = "฿", BaseRate = 3.10 },
                    new() { Name = "Indonesian Rupiah", Symble = "Rp", BaseRate = 0.007 },

                    // 🇦🇺 Oceania
                    new() { Name = "Australian Dollar", Symble = "$", BaseRate = 73.00 },
                    new() { Name = "New Zealand Dollar", Symble = "$", BaseRate = 68.00 },

                    // 🇸🇦 Middle East
                    new() { Name = "Saudi Riyal", Symble = "﷼", BaseRate = 29.30 },
                    new() { Name = "UAE Dirham", Symble = "د.إ", BaseRate = 30.00 },
                    new() { Name = "Qatari Riyal", Symble = "﷼", BaseRate = 30.20 },
                    new() { Name = "Kuwaiti Dinar", Symble = "د.ك", BaseRate = 360.00 },
                    new() { Name = "Omani Rial", Symble = "﷼", BaseRate = 286.00 },

                    // 🌍 Africa
                    new() { Name = "South African Rand", Symble = "R", BaseRate = 6.00 },
                    new() { Name = "Egyptian Pound", Symble = "£", BaseRate = 3.55 },
                    new() { Name = "Nigerian Naira", Symble = "₦", BaseRate = 0.14 }
                }
                .Select(c => new Currency
                {
                    Name = c.Name.Trim(),
                    Symble = c.Symble,
                    BaseRate = Math.Round(c.BaseRate, 4),
                    IsDeleted = false,
                    DeletedDateTime = null
                })
                .ToList();
            }
        }
    }
}