# Tournament

Programinis sprendimas išskirstytas į skirtingus programinio kodo sluoksnius. 
1. Pirmasis sluoksnis Client savyje turi turėti programinį kodą susieta su naudotojo sąsaja. Client sluoksnyje yra laikomos pakopinių stiliaus kalbos (CSS) rinkmenos, Blazor technologijos RAZOR rinkmenų plėtiniai ir service tipo klasės skirtos komunikacijai su sistemos REST API serveriu.
2. Antrame sluoksnyje Domain laikomos visos esybių klasės skirtos darbui su duomenų baze.
3. Trečiame programinio sprendimo sluoksnyje Domain.Services laikomos verslo logikos klasės. Šios verslo logikos klasės atlieka užklausas ir pakeitimus duomenų bazėje, atlieka verslo logikos veiksmus susijusius su badmintono turnyro organizavimu ir duomenų validacijomis.
4. Infrastructure sluoksnyje laikomos duomenų bazės migracijos rinkmenos, duomenų bazės lentelių konfigūracija ir sąsaja su esybių klasėmis.
5. Server sluoksnyje laikomos Controller klasės, kurios atsakingos už tarpininkavimą tarp naudotojo sąsajos projekto ir verslo logikos klasių. Čia aprašomi REST API metodai. 
6. Shared sluoksnyje laikomi bendri modeliai, kurie skirti bendravimui tarp Client ir Server projektų. 
