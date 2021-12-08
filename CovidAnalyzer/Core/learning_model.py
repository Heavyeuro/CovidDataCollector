from sktime.utils.plotting import plot_series
import numpy as np
import pandas as pd
from pandas import DataFrame
from sklearn.impute import SimpleImputer
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import mean_absolute_error, mean_squared_error
from sklearn.preprocessing import MinMaxScaler
from keras.models import Sequential
from keras.layers import Dense, LSTM
from sklearn.metrics import accuracy_score


def create_model(x_train_, y_train_, x_test_, y_test_, i):
    #Build the LSTM model
    model = Sequential()
    model.add(LSTM(128, return_sequences=True, input_shape=(x_train_.shape[1], x_train_.shape[2])))  # added _ to x_train_
    model.add(LSTM(64, return_sequences=False))
    model.add(Dense(25))
    model.add(Dense(1))

    # Compile the model
    model.compile(optimizer ='adam', loss='mean_squared_error')

    #Train the model
    model.fit(x_train_, y_train_, batch_size=1, epochs=i, validation_data=(x_test_, y_test_))

    model.summary()
    return model


def build_and_score_ml_model_core(df: DataFrame):
    model = build_estimate(df)
    print()


def plot(train_size, univariate_df, test_predict):
    x_train, y_train = pd.DataFrame(univariate_df.iloc[:train_size, 0]), pd.DataFrame(univariate_df.iloc[:train_size, 1])
    x_valid, y_valid = pd.DataFrame(univariate_df.iloc[train_size:, 0]), pd.DataFrame(univariate_df.iloc[train_size:, 1])

    y_pred = pd.DataFrame(test_predict[:,0])
    y_pred.index = x_valid.index
    plot_series(y_train, y_valid, y_pred, labels=["y_train", "y_test", "y_pred"])
    # plt.show()


def build_estimate(df):
    train_size = int(0.85 * len(df))
    test_size = len(df) - train_size

    univariate_df = df[['date', 'new_cases']].copy()
    univariate_df.columns = ['ds', 'y']

    train = univariate_df.iloc[:train_size, :]

    x_train, y_train = pd.DataFrame(univariate_df.iloc[:train_size, 0]), pd.DataFrame(univariate_df.iloc[:train_size, 1])
    x_valid, y_valid = pd.DataFrame(univariate_df.iloc[train_size:, 0]), pd.DataFrame(univariate_df.iloc[train_size:, 1])

    data = univariate_df.filter(['y'])
    #Convert the dataframe to a numpy array
    dataset = data.values

    scaler = MinMaxScaler(feature_range=(-1, 0))
    scaled_data = scaler.fit_transform(dataset)

    # Defines the rolling window
    look_back = 52
    # Split into train and test sets
    train, test = scaled_data[:train_size-look_back,:], scaled_data[train_size-look_back:,:]

    def create_dataset(dataset, look_back=1):
        X, Y = [], []
        for i in range(look_back, len(dataset)):
            a = dataset[i-look_back:i, 0]
            X.append(a)
            Y.append(dataset[i, 0])
        return np.array(X), np.array(Y)

    x_train, y_train = create_dataset(train, look_back)
    x_test, y_test = create_dataset(test, look_back)

    # reshape input to be [samples, time steps, features]
    x_train = np.reshape(x_train, (x_train.shape[0], 1, x_train.shape[1]))
    x_test = np.reshape(x_test, (x_test.shape[0], 1, x_test.shape[1]))

    best_rmse = 100
    best_epoch = 0
    for i in range(1, 20):
        x_test_ = x_test
        x_train_ = x_train
        y_train_ = y_train
        y_test_ = y_test
        model = create_model(x_train_, y_train_, x_test_, y_test_, i)
        # Lets predict with the model
        train_predict_ = model.predict(x_train)
        test_predict_ = model.predict(x_test)
        # invert predictions
        train_predict_ = scaler.inverse_transform(train_predict_)
        y_train_ = scaler.inverse_transform([y_train_])

        test_predict_ = scaler.inverse_transform(test_predict_)
        y_test_ = scaler.inverse_transform([y_test_])

        # Get the root mean squared error (RMSE) and MAE
        score_rmse = np.sqrt(mean_squared_error(y_test_[0], test_predict_[:, 0]))
        score_mae = mean_absolute_error(y_test_[0], test_predict_[:, 0])
        print(len(x_train), len(y_train))
        print(len(x_valid), len(y_valid))
        if best_rmse > score_rmse:
                 best_rmse = score_rmse
                 best_epoch = i
    print(best_epoch)
    print('RMSE: {}'.format(best_rmse))

    return model # -> find the best model
    #
    # x_train, y_train = pd.DataFrame(univariate_df.iloc[:train_size, 0]), pd.DataFrame(univariate_df.iloc[:train_size, 1])
    # x_valid, y_valid = pd.DataFrame(univariate_df.iloc[train_size:, 0]), pd.DataFrame(univariate_df.iloc[train_size:, 1])
    #
    # y_pred = pd.DataFrame(test_predict_[:, 0])
    # y_pred.index = x_valid.index
    # plot_series(y_train, y_valid, y_pred, labels=["y_train", "y_test", "y_pred"])
    #
    #
    #
    #
    #
    # train_size = 60
    # fh_initial_value = df.index[-1]
    # univariate_df = DataFrame(data=np.arange(fh_initial_value, fh_initial_value + train_size+1), index=np.arange(fh_initial_value, fh_initial_value + train_size+1))
    # univariate_df.columns = ['y']
    #
    # data = univariate_df.filter(['y'])
    # #Convert the dataframe to a numpy array
    # dataset = data.values
    #
    # scaler = MinMaxScaler(feature_range=(-1, 0))
    # scaled_data = scaler.fit_transform(dataset)
    # # Defines the rolling window
    # look_back = 52
    # # Split into train and test sets
    # train, test = scaled_data[:train_size-look_back,:], scaled_data[train_size-look_back:,:]
    # x_test, y_test = create_dataset(test, look_back)
    #
    # # reshape input to be [samples, time steps, features]
    # x_test = np.reshape(x_test, (x_test.shape[0], 1, x_test.shape[1]))
    # further_prediction = model.predict(x_test)
