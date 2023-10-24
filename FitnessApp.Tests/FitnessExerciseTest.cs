using AutoMapper;
using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    public class FitnessExerciseTest
    {
        private readonly FitnessExerciseController fitnessExerciseController;
        private readonly Mock<IFitnessExerciseService> _fitnessExerciseServiceMock = new Mock<IFitnessExerciseService>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

        public FitnessExerciseTest()
        {
            fitnessExerciseController = new FitnessExerciseController(
                null,
                _fitnessExerciseServiceMock.Object,
                mockMapper.Object
                );
        }

        [Fact]
        public async Task GetAsync_ReturnsListOfFitnessExerciseDto()
        {
            // Arrange
            var mockExercises = new List<FitnessExercise>();
            var mockExerciseDtos = new List<FitnessExerciseDto>();

            _fitnessExerciseServiceMock.Setup(service => service.GetFitnessExercisesAsync()).ReturnsAsync(mockExercises);
            mockMapper.Setup(mapper => mapper.Map<List<FitnessExerciseDto>>(mockExercises)).Returns(mockExerciseDtos);

            // Act
            var result = await fitnessExerciseController.GetAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<FitnessExerciseDto>>>(result);
            var model = Assert.IsType<List<FitnessExerciseDto>>(actionResult.Value);
            Assert.Equal(mockExerciseDtos.Count, model.Count);
            _fitnessExerciseServiceMock.Verify(service => service.GetFitnessExercisesAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsFitnessExerciseDto()
        {
            // Arrange
            var exerciseId = 10;
            var mockExercise = new FitnessExercise();
            var mockExerciseDto = new FitnessExerciseDto();

            _fitnessExerciseServiceMock.Setup(service => service.GetFitnessExerciseAsync(exerciseId)).ReturnsAsync(mockExercise);
            mockMapper.Setup(mapper => mapper.Map<FitnessExerciseDto>(mockExercise)).Returns(mockExerciseDto);

            // Act
            var result = await fitnessExerciseController.Get(exerciseId);

            // Assert
            var exerciseDto = Assert.IsType<FitnessExerciseDto>(result);
            Assert.Equal(mockExerciseDto, exerciseDto);
            _fitnessExerciseServiceMock.Verify(service => service.GetFitnessExerciseAsync(exerciseId), Times.Once);
        }

        [Fact]
        public async Task AddFitnessExercise_ReturnsOkResultWithExerciseId()
        {
            // Arrange
            var mockExerciseDto = new FitnessExerciseDto();
            var mockExercise = new FitnessExercise { Id = 10 };

            _fitnessExerciseServiceMock.Setup(service =>
                service.InsertFitnessExerciseAsync(It.IsAny<FitnessExercise>())).ReturnsAsync(mockExercise);

            mockMapper.Setup(mapper => mapper.Map<FitnessExercise>(mockExerciseDto)).Returns(mockExercise);

            // Act
            var result = await fitnessExerciseController.AddFitnessExercise(mockExerciseDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockExercise.Id, okObjectResult.Value);
            _fitnessExerciseServiceMock.Verify(service => service.InsertFitnessExerciseAsync(mockExercise), Times.Once);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult()
        {
            // Arrange
            var exerciseId = 10;
            var mockExerciseDto = new FitnessExerciseDto();

            // Act
            var result = await fitnessExerciseController.Put(exerciseId, mockExerciseDto);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
            _fitnessExerciseServiceMock.Verify(service => service.UpdateFitnessExerciseAsync(exerciseId, It.IsAny<FitnessExercise>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            var exerciseId = 10;

            // Act
            var result = await fitnessExerciseController.Delete(exerciseId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
            _fitnessExerciseServiceMock.Verify(service => service.DeleteFitnessExerciseAsync(exerciseId), Times.Once);
        }


    }
}
