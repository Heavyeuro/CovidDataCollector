import DatasetAnalysis as DA
import DatasetInterpolation as DI
import Trend as td

if __name__ == '__main__':
    name = 'covidData.csv'
    # DI.main_handle_dataset(name)
    td.main_trend_development(name)
