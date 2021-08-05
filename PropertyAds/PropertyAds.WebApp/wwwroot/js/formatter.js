class Formatter {
    static currencyEuro(value) {
        return new Intl.NumberFormat(
            'de-DE',
            { style: 'currency', currency: 'EUR', maximumSignificantDigits: 6 })
                .format(value);
    }
}