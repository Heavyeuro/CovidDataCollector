import prepare_dataset as pds
import learning_model as lm


if __name__ == '__main__':
    patients_info_filename = 'covidData.csv'

    dataframe = pds.prepare_dataset_core(patients_info_filename)

    # building ML
    lm.build_and_score_ml_model_core(dataframe)
