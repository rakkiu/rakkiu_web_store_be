using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
namespace Infrastructure.Repositories
{
    /// <summary>
    /// User repository implementation
    /// </summary>
    /// <seealso cref="IAMService.Domain.Interfaces.IUserRepository" />
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public UserRepository(ApplicationDbContext db) => _db = db;

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            user.Email = EncryptionHelper.EncryptDeterministic(user.Email);
            user.Phone = EncryptionHelper.Encrypt(user.Phone);
            user.FullName = EncryptionHelper.Encrypt(user.FullName);
            user.Address = EncryptionHelper.Encrypt(user.Address);

            await _db.Users.AddAsync(user, ct);
        }

        /// <summary>
        /// Gets the by email asynchronous.
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(email);
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == encryptedEmail, ct);

            if (user != null)
                DecryptUserSensitiveData(user);

            return user;
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _db.Users
                .Include(u => u.JwtTokens)
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            if (user != null)
                DecryptUserSensitiveData(user);

            return user;
        }

        /// <summary>
        /// Gets the by identifier with role asynchronous.
        /// </summary>
        public async Task<User?> GetByIdWithRoleAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _db.Users
                .Include(u => u.JwtTokens)
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            if (user != null)
                DecryptUserSensitiveData(user);

            return user;
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        /// <summary>
        /// Gets the list user.
        /// </summary>
        public async Task<IEnumerable<User>> GetListUser()
        {
            var users = await _db.Users.ToListAsync();
            foreach (var u in users)
                DecryptUserSensitiveData(u);

            return users;
        }

        /// <summary>
        /// Marks an existing user entity as modified (Soft Delete or Update).
        /// </summary>
        public void Update(User user)
        {
            user.Email = EncryptionHelper.EncryptDeterministic(user.Email);
            user.Phone= EncryptionHelper.Encrypt(user.Phone);
            user.FullName = EncryptionHelper.Encrypt(user.FullName);
            user.Address = EncryptionHelper.Encrypt(user.Address);

            _db.Users.Update(user);
        }

        /// <summary>
        /// Marks an existing user entity for removal (Hard Delete).
        /// </summary>
        public void Remove(User user)
        {
            _db.Users.Remove(user);
        }

        /// <summary>
        /// Decrypts user sensitive data.
        /// </summary>
        private void DecryptUserSensitiveData(User user)
        {
            user.Email = EncryptionHelper.DecryptDeterministic(user.Email);
            // Use Decrypt (random IV) for Phone and Address as they are encrypted with Encrypt() in AddAsync
            if (!string.IsNullOrEmpty(user.Phone)) user.Phone = EncryptionHelper.Decrypt(user.Phone);
            if (!string.IsNullOrEmpty(user.Address)) user.Address = EncryptionHelper.Decrypt(user.Address);
        }

        /// <summary>
        /// Gets the users by role code asynchronous.
        /// </summary>


        /// <summary>
        /// Update without encryption (used for internal versioned logic).
        /// </summary>
        public void UpdateV1(User user)
        {
            _db.Users.Update(user);
        }

        /// <summary>
        /// Updates only the password field without re-encrypting other fields.
        /// </summary>
        public void UpdatePasswordOnly(User user)
        {
            var entry = _db.Entry(user);
            entry.Property(u => u.PasswordHash).IsModified = true;
        }

        /// <summary>
        /// Gets the by identifier without decrypt asynchronous.
        /// </summary>
        public async Task<User?> GetByIdWithoutDecryptAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        /// <summary>
        /// Gets user by email WITHOUT decrypting sensitive data (for internal operations only).
        /// </summary>
        public async Task<User?> GetByEmailWithoutDecryptAsync(string email, CancellationToken ct = default)
        {
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(email);
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == encryptedEmail, ct);
        }
    }
}
