﻿using Business.Abstract;
using Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.DTOs;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Business.Constants;
using DataAccess.Concrete.EntityFramework;
using System.Linq;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;

namespace Business.Concrete
{
	public class CarManager : ICarService
	{
		ICarDal _carDal;

		public CarManager(ICarDal carDal)
		{
			_carDal = carDal;
		}

		public IDataResult<List<Car>> GetAll()
		{
            if (DateTime.Now.Hour==16)
            {
				return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }
            else
            {
				return new SuccessDataResult<List<Car>>(_carDal.GetAll());
			}

		}

		public IDataResult<List<Car>> GetCarsByBrandId(int id)
		{
			return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == id));
		}

		public IDataResult<List<Car>> GetCarsByColorId(int id)
		{
			return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.ColorId == id));
		}

		public IDataResult<List<CarDetailDto>> GetCarDetails()
		{
			return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails());
		}

		[ValidationAspect(typeof(CarValidator))]
		public IResult Add(Car car)
        {
			if (car.Description.Length < 2)
			{
				return new ErrorResult(Messages.CarNameInvalid);
			}
			else if (car.DailyPrice < 0)
			{
				return new ErrorResult(Messages.DailyPriceInvalid);
			}
			else
			{
				_carDal.Add(car);
				return new SuccessResult(Messages.CarAdded);
			}


		}

		public IResult Update(Car car)
        {
			_carDal.Update(car);
			return new SuccessResult(Messages.CarUpdated);
        }

        public IResult Delete(Car car)
        {
			_carDal.Delete(car);
			return new SuccessResult(Messages.CarDeleted);
        }

        public IDataResult<Car> GetById(int id)
        {
			return new SuccessDataResult<Car>(_carDal.Get(b => b.Id == id));
		}
    }
}
