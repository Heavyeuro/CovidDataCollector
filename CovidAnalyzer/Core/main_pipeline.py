import data_analysis as da
import prepare_dataset as pds
import learning_model as lm
import core_action as ca


if __name__ == '__main__':
    patients_info_filename = 'covidData.csv'

    # prepare generated DF(values replacing, imputing)
    dataframe = pds.prepare_dataset_core(patients_info_filename)

    # build graphs and analyze dataset
    da.describe_dataframe_core(dataframe)

    # building ML
    lm.build_and_score_ml_model_core(dataframe)
