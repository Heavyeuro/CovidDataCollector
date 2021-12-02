from statistics import mean
from matplotlib import pyplot as plt, pyplot
from numpy import std
from pandas import DataFrame
from xgboost import XGBRegressor
from sklearn.ensemble import RandomForestRegressor, RandomForestClassifier
from sklearn.metrics import mean_absolute_error, accuracy_score, confusion_matrix, ConfusionMatrixDisplay
from sklearn.model_selection import train_test_split, GridSearchCV, RepeatedStratifiedKFold, cross_val_score
import core_action as ca


# beside_list - columns to avoid
def build_and_score_ml_model_core(X_full: DataFrame, beside_list=[]):
    target_column = 'diagnostic_superclass'
    y = X_full[target_column]
    X = ca.null_to_NaN(X_full.drop([target_column], axis=1), beside_list)
    X_train, X_valid, y_train, y_valid = train_test_split(X, y, train_size=0.83, test_size=0.17, random_state=5)



def build_confusion_matrix(predictions, y_test):
    cm = confusion_matrix(y_test, predictions)
    disp = ConfusionMatrixDisplay(confusion_matrix=cm)
    disp.plot()
    plt.show()


