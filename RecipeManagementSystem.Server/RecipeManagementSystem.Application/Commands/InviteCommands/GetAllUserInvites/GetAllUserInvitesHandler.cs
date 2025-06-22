using AutoMapper;
using MediatR;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.GetAllUserInvites;

public class GetAllUserInvitesHandler(IGenericRepository<CollaborationInvite> repository, IMapper mapper)
    : IRequestHandler<GetAllUserInvitesQuery, Result<IEnumerable<CollaborationInviteDto>>>
{
    public async Task<Result<IEnumerable<CollaborationInviteDto>>> Handle(GetAllUserInvitesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
        {
            return Result<IEnumerable<CollaborationInviteDto>>.Failure(RequestErrors.InvalidId);
        }
        
        var invites = repository.GetAllByFilter<CollaborationInviteDto>(i => i.InviterId == request.UserId);

        return invites;
    }
}