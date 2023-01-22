# Copyright 2013 Walter Scheper
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

import hashlib
import logging
import os
import re
import sys

from cliff import command

from . import __version__


FORMAT_ERROR = 'MISFORMATTED'
HASH_ERROR = 'FAILED'
READ_ERROR = 'FAILED open or read'
SUCCESS = 'OK'
EXCLUDES = ('base.py', '__init__.py')
PATH = os.path.dirname(__file__)
PYEXT = '.py'
STATUS_MSG = '{0}: {1}\n'


class Hasher(command.Command):
    """
    Base class for various sub-classes that implement specific hashing
    algorithms.
    """

    log = logging.getLogger(__name__)

    def __init__(self, *args, **kwargs):
        super(Hasher, self).__init__(*args, **kwargs)
        self.chunk_size = 64 * 2048

    def _calculate_hash(self, file_object):
        """
        Calculate a hash value for the data in ``file_object
        """
        hasher = self.hashlib()
        for chunk in self.iterchunks(file_object):
            hasher.update(chunk)
        return hasher.hexdigest()

    def _open_file(self, fname, binary=False):
        if fname == '-':
            return sys.stdin
        return open(fname, 'rb' if binary else 'r')

    def check_hash(self, fname, args):
        """
        Check the hashed values in files against the calculated values

        Returns a list and a tuple of error counts.

        list: [(fname, 'OK' or 'FAILED' or 'FAILED open or read'),...]
        Error counts: (format_erros, hash_errors, read_errors)
        """
        fobj = self._open_file(fname)

        rc = 0
        format_errors = 0
        hash_errors = 0
        read_errors = 0
        for idx, line in enumerate(fobj):
            # remove any newline characters
            m = self.CHECK_RE.match(line.strip())
            if not m:
                if args.warn:
                    self.app.stderr.write(
                        'hasher {0}: {1}: {2}: improperly formatted {3}'
                        ' checksum line\n'.format(self.name, fname, idx + 1,
                                                  self.name.upper()))
                format_errors += 1
                rc = 1
                continue
            hash_value, binary, check_file = m.groups()

            try:
                check_f = open(check_file, 'rb' if binary == '*' else 'r')
            except IOError:
                self.app.stderr.write(
                    'hasher {0}: {1}: No such file or directory\n'.format(
                        self.name, check_file))
                if not args.status:
                    self.app.stdout.write(
                        STATUS_MSG.format(check_file, READ_ERROR))
                read_errors += 1
                rc = 1
                continue

            if self._calculate_hash(check_f) == hash_value:
                if not (args.quiet or args.status):
                    self.app.stdout.write(
                        STATUS_MSG.format(check_file, SUCCESS))
            else:
                if not args.status:
                    self.app.stdout.write(
                        STATUS_MSG.format(check_file, HASH_ERROR))
                hash_errors += 1
                rc = 1

        if format_errors and not args.status:
            self.app.stderr.write(
                'hasher {0}: WARNING: {1} line{2} {3} improperly'
                ' formatted\n'.format(
                    self.name,
                    format_errors,
                    's' if format_errors > 1 else '',
                    'are' if format_errors > 1 else 'is',
                    ))
        if read_errors and not args.status:
            self.app.stderr.write(
                'hasher {0}: WARNING: {1} listed file{2}'
                ' could not be read\n'.format(
                    self.name,
                    read_errors,
                    's' if read_errors > 1 else '',
                    ))
        if hash_errors and not args.status:
            self.app.stderr.write(
                'hasher {0}: WARNING: {1} computed checksum{2}'
                ' did NOT match\n'.format(
                    self.name,
                    hash_errors,
                    's' if hash_errors > 1 else '',
                    ))
        return rc

    def generate_hash(self, fname, args):
        """Generate hashes for files"""
        fobj = self._open_file(fname, args.binary)
        hash_value = self._calculate_hash(fobj)

        line = '{0} {1}{2}\n'.format(hash_value, '*' if args.binary else ' ',
                                     fname)

        if '//' in line:
            line = '//' + line.replace('//', '////')
        self.app.stdout.write(line)

    def get_parser(self, prog_name):
        parser = super(Hasher, self).get_parser(prog_name)
        parser.add_argument(
            "file",
            nargs="*",
            default="-",
            metavar="FILE",
            )
        parser.add_argument(
            "-c", "--check",
            action="store_true",
            help="read MD5 sums from the FILEs and check them",
            )
        parser.add_argument(
            "-b", "--binary",
            action="store_true",
            help="read in binary mode",
            )
        parser.add_argument(
            "-t", "--text",
            action="store_true",
            help="read in text mode (default)",
            )
        parser.add_argument(
            "--quiet",
            action="store_true",
            help="don't print OK for each successfully verified file",
            )
        parser.add_argument(
            "--status",
            action="store_true",
            help="don't output anything, status code shows success",
            )
        parser.add_argument(
            "-w", "--warn",
            action="store_true",
            help="warn about improperly formatted checksum lines",
            )
        parser.add_argument(
            "--strict",
            action="store_true",
            help="with --check, exit non-zero for any invalid input",
            )
        parser.add_argument(
            "--version",
            action="version",
            version="{0}.{1}.{2}{3}".format(*__version__),
            )
        parser.add_argument(
            '--debug',
            action='store_true',
            help='Show debugging statements',
            )
        return parser

    def iterchunks(self, file_object):
        data = file_object.read(self.chunk_size)
        while data != '':
            yield data
            data = file_object.read(self.chunk_size)

    def take_action(self, parsed_args):
        if parsed_args.check and (parsed_args.binary and parsed_args.text):
            raise RuntimeError(
                'the --binary and --text options are meaningless when'
                ' verifying checksums')

        if not parsed_args.check and (
                parsed_args.warn or
                parsed_args.status or
                parsed_args.quiet or
                parsed_args.strict):
            raise RuntimeError(
                'the --warn, --status, and --quiet options are meaningful'
                ' only when verifying checksums')

        for fname in parsed_args.file:
            if parsed_args.check:
                self.check_hash(fname, parsed_args)
            else:
                self.generate_hash(fname, parsed_args)


class MD5Hasher(Hasher):
    CHECK_RE = re.compile(r'^([a-f0-9]{32}) (\*| )(.+)$')
    name = 'md5'
    hashlib = hashlib.md5


class SHA1Hasher(Hasher):
    CHECK_RE = re.compile(r'^([a-f0-9]{40}) (\*| )(.+)$')
    name = 'sha1'
    hashlib = hashlib.sha1


class SHA256Hasher(Hasher):
    CHECK_RE = re.compile(r'^([a-f0-9]{64}) (\*| )(.+)$')
    name = 'sha256'
    hashlib = hashlib.sha256
